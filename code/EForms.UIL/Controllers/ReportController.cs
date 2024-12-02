using Newtonsoft.Json;
using Performance.Management.BLL.Core;
using Performance.Management.BLL.Helper;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.MainEntities;
using Performance.Management.DML.Models;
using Performance.Management.UIL.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Performance.Management.UIL.Controllers
{
    public class ReportController : BaseController
    {
        #region Variables
        readonly InMemoryCache memoryCache = new InMemoryCache();

        readonly Employee _Employee = new Employee();
        readonly OasisSync _OasisSync = new OasisSync();

        readonly RequestLogic _RequestLogic = new RequestLogic();
        readonly RequestModel _RequestModel = new RequestModel();
        HomeModel _HomeModel = new HomeModel();
        readonly Permission _Permission = new Permission();
        readonly StatusLogic _StatusLogic = new StatusLogic();

        readonly EmpDetails _EmpDetails = new EmpDetails();
        readonly Entities myDB = new Entities();
        #endregion

        #region Data-table
        public ActionResult Index()
        {
            if (IsAllowedUser())
            {
                _RequestModel.StatusList = _StatusLogic.GetAll().Where(x => !x.Name.Contains("Seen By")).ToList();
                _RequestModel.Department = _OasisSync.GetAllDepartment();

                return View(_RequestModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(JQueryDataTableParams Params, string workEntity, string st)
        {
            string userName = GetUserName();

            string lang = base.GetCurrentLanguage();

            List<RequestDatatable> requests = _RequestLogic.GetAllRequests(lang, userName, Params, workEntity, int.Parse(st));

            List<EmpSearch> empData = GetEmployeesByWorkEntity(workEntity);

            if (st == "-1")
            {
                List<string> empAppiedIds = requests.Select(x => x.StaffID).ToList();

                List<EmpSearch> empDataNotApplied = empData.Where(x => !empAppiedIds.Contains(x.STAFF_ID)).ToList();

                foreach (var emp in empDataNotApplied)
                {
                    bool isSupervisor = myDB.SupervisorUsers.Any(x => x.IsSupervisor == true && x.UserId == emp.STAFF_ID);

                    string type = Resource.NonSupervisory;

                    if (isSupervisor)
                    {
                        type = Resource.Supervisory;
                    }

                    requests.Add(new DML.Models.RequestDatatable
                    {
                        ID = 0,
                        AllowDelete = new DML.Models.AllowDelete
                        {
                            CanAction = false,
                            CanDelete = false,
                            IsFinished = false,
                            IsRequestUserName = false,
                            RequestID = 0,
                            NotAppied_STAFF_ID = emp.STAFF_ID
                        },
                        EmployeeUserEnglishName = emp.STAFF_NAME,
                        EmployeeUserName = emp.DOC_NUMBER,
                        StaffID = emp.STAFF_ID,
                        RequestStatus = new DML.Models.RequestStatus
                        {
                            StatusID = 4,
                            StatusName = Resource.PerformanceNotApplied
                        },
                        RequestDeptEnglishName = emp.DEPT_NAME,
                        RequesterUserEnglishName = emp.MANAGER_NAME,
                        DateTime = DateTime.MinValue.ToString("dd/MM/yyyy"),
                        DeadlineDate = DateTime.MinValue.ToString("dd/MM/yyyy"),
                        RequestType = type,
                        TypeID = 0
                    });
                }
            }

            //List<RequestDatatable> requestsToReturn = new List<RequestDatatable>();

            //foreach (var r in requests)
            //{
            //    var _emp = empData.FirstOrDefault(x => x.DOC_NUMBER == r.EmployeeUserName);

            //    if (_emp != null && string.IsNullOrEmpty(r.StaffID))
            //    {
            //        r.StaffID = _emp.STAFF_ID;
            //    }

            //    requestsToReturn.Add(r);
            //}

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = requests.Count,
                iTotalDisplayRecords = requests.Count,
                aaData = requests
            },
               JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region EmpData - Datatable
        public ActionResult EmpData()
        {
            if (!IsAllowedUser())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            EmpDataModel empDataModel = new EmpDataModel();
            //empDataModel.Header = base.GetEmpList().FirstOrDefault();
            empDataModel.Department = _OasisSync.GetAllDepartment();

            return View(empDataModel);
        }

        [HttpPost]
        public string GetEmpData(JQueryDataTableParams Params, string workEntity)
        {
            string userName = GetUserName();

            List<EmpSearch> empData = GetEmployeesByWorkEntity(workEntity);

            var datatable = new
            {
                sEcho = Params.sEcho,
                iTotalRecords = empData.Count(),
                iTotalDisplayRecords = empData.Count(),
                aaData = empData
            };

            return JsonConvert.SerializeObject(datatable);
        }

        private List<EmpSearch> GetEmployeesByWorkEntity(string workEntity)
        {
            var empData = base.GetEmpList().Where(d => d.CONTRACT_TYPE_DESC.Equals("INTERNATIONAL", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_DESC.Equals("LOCAL (40 DAYS)", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_DESC.Equals("LOCAL (30 DAYS)", StringComparison.OrdinalIgnoreCase)).ToList();

            if (workEntity != "-1")
            {
                var tempEmpData = new List<EmpSearch>();

                foreach (var (item, index) in empData.WithIndex())
                {
                    var entityCode = item.WORK_ENTITY.ToString().TrimEnd();
                    var match = Regex.Match(entityCode, @"[0-9]+\.[0-9]+");
                    if (match.Success)
                    {
                        entityCode = match.Value;
                    }

                    if (entityCode == workEntity)
                    {
                        var supUser = myDB.SupervisorUsers.FirstOrDefault(x => x.IsSupervisor == true && x.UserId == item.STAFF_ID);

                        if (supUser != null)
                        {
                            item.IsSupervisor = true;
                            item.SupervisorUserId = supUser.ID;
                        }

                        tempEmpData.Add(item);
                    }
                }

                empData = tempEmpData;
            }

            return empData;
        }

        private EmpSearch GetEmpSearch(string docNumnber)
        {
            var emp = base.GetEmpList().FirstOrDefault(x => x.DOC_NUMBER == docNumnber &&
                (x.CONTRACT_TYPE_DESC.Equals("INTERNATIONAL", StringComparison.OrdinalIgnoreCase) || x.CONTRACT_TYPE_DESC.Equals("LOCAL (40 DAYS)", StringComparison.OrdinalIgnoreCase) || x.CONTRACT_TYPE_DESC.Equals("LOCAL (30 DAYS)", StringComparison.OrdinalIgnoreCase)));

            return emp;
        }
        #endregion

        #region Emp Search
        public ActionResult EmpSearch(string Id = null)
        {
            if (!IsAllowedUser())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (string.IsNullOrEmpty(Id))
            {
                return View(new EmpDetails());
            }

            var emp = _OasisSync.GetEmployeeByNationalIdOrStaffId(Id);

            _EmpDetails.Id = Id;
            _EmpDetails.Employee = emp;
            _EmpDetails.UserId = emp.StaffId;

            if (emp != null && emp.StaffId != null)
            {
                var supUser = myDB.SupervisorUsers.FirstOrDefault(x => x.IsSupervisor == true && x.UserId == emp.StaffId);

                if (supUser != null)
                {
                    _EmpDetails.IsSupervisor = true;
                    _EmpDetails.SupervisorUserId = supUser.ID;
                    _EmpDetails.UserId = supUser.UserId;
                }

                var requests = myDB.Requests.Where(x => x.Employee_UserName == emp.DocNumber && x.IsDeleted == false).ToList();

                if (requests != null)
                {
                    _EmpDetails.Requests = requests;
                }
            }

            return View(_EmpDetails);
        }

        #endregion
    }
}