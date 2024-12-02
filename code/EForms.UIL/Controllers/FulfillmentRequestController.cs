#region Using

using Performance.Management.BLL.Core;
using Performance.Management.BLL.Helper;
using Performance.Management.BLL.ViewModel;
using Performance.Management.BLL.ViewModel.Enums;
using Performance.Management.DML.MainEntities;
using Performance.Management.UIL.Language;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

#endregion

namespace Performance.Management.UIL.Controllers
{
    public class FulfillmentRequestController : BaseController
    {
        #region Variables

        readonly RequestModel _RequestModel = new RequestModel();
        readonly RequestLogic _RequestLogic = new RequestLogic();
        readonly SActionLogic _SActionLogic = new SActionLogic();
        readonly ReasonLogic _ReasonLogic = new ReasonLogic();
        readonly StatusLogic _StatusLogic = new StatusLogic();

        readonly OasisSync _OasisSync = new OasisSync();
        readonly Email _Email = new Email();
        readonly Permission _Permission = new Permission();
        readonly Common _Common = new Common();

        readonly Entities myDB = new Entities();

        #endregion

        #region Data-table
        public ActionResult Index()
        {
            _RequestModel.StatusList = _StatusLogic.GetAll().Where(x => !x.Name.Contains("Seen By ")).ToList();

            return View(_RequestModel);
        }

        [HttpPost]
        public ActionResult Index(JQueryDataTableParams Params, string rt, string st)
        {
            string userName = GetUserName();
            int totalCount = 0;
            int TotalDisplayRecords = 0;
            object Result;

            string lang = base.GetCurrentLanguage();

            if (st.ToLower() == "pendingrequests")
            {
                Result = _RequestLogic.GetAllRequests(lang, userName, Params, int.Parse(rt), out totalCount, out TotalDisplayRecords);
            }
            else
            {
                Result = _RequestLogic.GetAllRequests(lang, userName, Params, int.Parse(rt), out totalCount, out TotalDisplayRecords, int.Parse(st));
            }

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = totalCount,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pending()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pending(JQueryDataTableParams Params)
        {
            string Name = GetUserName();
            string lang = base.GetCurrentLanguage();

            var Result = _RequestLogic.GetAllRequests(lang, Name, Params, 5, out int totalCount, out int TotalDisplayRecords);

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = TotalDisplayRecords,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Resolved(JQueryDataTableParams Params)
        {
            string Name = GetUserName();

            var Result = _RequestLogic.GetAllResolvedRequest(Name, Params, out int totalCount, out int TotalDisplayRecords).Select(M => new
            {
                ID = M.ID,
                RequesterDeptEnglishName = (!String.IsNullOrEmpty(M.RequesterDeptEnglishName)) ? M.RequesterDeptEnglishName : "",
                RequestStatus = new { StatusName = (M.Status != null) ? M.Status.Name : "", StatusID = M.StatusID },
                AllowDelete = new
                {
                    RequestID = M.ID,
                    IsRequestUserName = (M.RequesterUserName != null && M.RequesterUserName.ToLower() == GetUserName().ToLower()) ? "True" : "False",
                    CanDelete = (M.DeptHead_Approval == null /*&& M.ApproverApproval == null && M.PlanningAndDev_Finished == null*/) ? "True" : "False"
                },
                DateTime = (M.CreateDateTime != null) ? M.CreateDateTime.Value.ToString("dd/MM/yyyy") : null
            });

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = TotalDisplayRecords,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllRequest()
        {
            string name = GetUserName();

            if (_Permission.CheckUserIsHR(name))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult AllRequest(JQueryDataTableParams Params)
        {
            var Result = _RequestLogic.GetAllRequest(Params, out int totalCount, out int TotalDisplayRecords).Select(M => new
            {
                ID = M.ID,
                RequesterDeptEnglishName = (!String.IsNullOrEmpty(M.RequesterDeptEnglishName)) ? M.RequesterDeptEnglishName : "",
                Status = (M.Status != null) ? M.Status.Name : "",
                RequestStatus = new { StatusName = (M.Status != null) ? M.Status.Name : "", StatusID = M.StatusID },
                DateTime = (M.CreateDateTime != null) ? M.CreateDateTime.Value.ToString("dd/MM/yyyy") : null
            });

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = TotalDisplayRecords,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        public ActionResult Rejection()
        {
            string Name = GetUserName();

            if (_Permission.CheckUserIsHR(GetUserName()))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult Rejection(JQueryDataTableParams Params)
        {

            var Result = _RequestLogic.GetRejetedRequest(Params, out int totalCount, out int TotalDisplayRecords).Select(M => new
            {
                ID = M.ID,
                RequesterDeptEnglishName = (!String.IsNullOrEmpty(M.RequesterDeptEnglishName)) ? M.RequesterDeptEnglishName : "",
                Status = (M.Status != null) ? M.Status.Name : "",
                RequestStatus = new { StatusName = (M.Status != null) ? M.Status.Name : "", StatusID = M.StatusID },
                DateTime = (M.CreateDateTime != null) ? M.CreateDateTime.Value.ToString("dd/MM/yyyy") : null
            });

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = TotalDisplayRecords,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Deletion

        public ActionResult Deletion()
        {
            string Name = GetUserName();

            if (_Permission.CheckUserIsHR(Name))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult Deletion(JQueryDataTableParams Params)
        {

            var Result = _RequestLogic.GetAllDeletedRequest(Params, out int totalCount, out int TotalDisplayRecords).Select(M => new
            {
                ID = M.ID,
                DeleteFullName = (!String.IsNullOrEmpty(M.DeleteFullName)) ? M.DeleteFullName : "",
                //RequesterDeptEnglishName = (!String.IsNullOrEmpty(M.RequesterDeptEnglishName)) ? M.RequesterDeptEnglishName : "",
                //Status = (M.Status != null) ? M.Status.Name : "",
                //RequestStatus = new { StatusName = (M.Status != null) ? M.Status.Name : "", StatusID = M.StatusID },
                DeleteDateTime = (M.DeleteDateTime != null) ? M.DeleteDateTime.Value.ToString() : null
            });

            return Json(new
            {
                sEcho = Params.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = TotalDisplayRecords,
                aaData = Result
            },
               JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Request _Request = _RequestLogic.GetRequest(id);

                    if (_Request != null && _Request.StatusID == 1)
                    {
                        _Request.IsDeleted = true;
                        _Request.DeleteDateTime = DateTime.Now;
                        _Request.StatusID = (int)RequestStatusEnum.Deleted;

                        Employee _Employee = _OasisSync.GetEmployeeByUserName(GetUserName());
                        _Request.DeleteUserName = GetUserName();
                        _Request.DeleteFullName = _Employee.ENGL_STAFF_NAME;

                        _ = _RequestLogic.UpdateRequest(_Request);

                        Success(string.Format("Request <strong>({0})</strong> deleted successfully.", _Request.ID));

                        //RedirectToAction("Index", "Home");

                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
            }

            Danger("Failed to delete.");

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search

        [HttpGet]
        public ActionResult Search()
        {
            _RequestModel.Language = base.GetCurrentLanguage();

            string userName = GetUserName();

            ViewBag.WUserName = userName;
            ViewBag.FUserName = userName;

            var empList = _OasisSync.GetMyReportingEmployeesByIDentityNo(userName);

            if (empList.Any())
            {
                _RequestModel.MyEmployees = empList;
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(_RequestModel);
        }

        #endregion

        #region Draft

        [HttpGet]
        public ActionResult Draft(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Danger("Invalid ID!");
                return RedirectToAction("Index");
            }

            _RequestModel.Language = base.GetCurrentLanguage();

            string userName = GetUserName();

            ViewBag.WUserName = userName;
            ViewBag.FUserName = userName;

            bool isRequestExist = int.TryParse(Id, out int _requestId) && myDB.Requests.Any(x => x.ID == _requestId) && Id.Length < 10;

            if (isRequestExist)
            {
                Request _request = myDB.Requests.FirstOrDefault(x => x.ID == _requestId);

                _RequestModel.SelectedEmployee = _OasisSync.GetEmployeeByUserName(_request.Employee_UserName);

                _RequestModel.MyEmployees = new List<Employee>();
                _RequestModel.StatusList = new List<Status>();
                _RequestModel.SActionList = new List<SAction>();
                _RequestModel.ReasonList = new List<Reason>();
                _RequestModel.Manager = new RequestManager();
                _RequestModel.Goals = new List<GoalViewModel>();
                _RequestModel.Competencies = new List<CompetencyViewModel>();
                _RequestModel.CurrentUserName = userName;
                _RequestModel.IsHRUser = _Permission.CheckUserIsHR(userName);

                _RequestModel.RequestTypes = myDB.RequestTypes.ToList();

                _RequestModel.IsRequestExist = true;
                _RequestModel.FormSubmit = true;

                var requestType = (RequestTypeEnum)_request.RequestTypeID;

                _RequestModel.Request = _request;
                _RequestModel.Grade = requestType;
                _RequestModel.Year = _request.Year;
                _RequestModel.RequestTypeID = _request.RequestTypeID;

                return View(_RequestModel);
            }
            else
            {
                if (_Permission.CheckCurrentUserIsManager(userName))
                {
                    EvaluateView(Id, userName, false);

                    return View(_RequestModel);
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
        }

        [HttpPost]
        public ActionResult Draft(string Id, RequestModel Model)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Danger("Invalid ID!");
                return RedirectToAction("Index");
            }

            bool isRequestExist = int.TryParse(Id, out int _requestId) && myDB.Requests.Any(x => x.ID == _requestId) && Id.Length < 10;

            if (isRequestExist)
            {
                return RedirectToAction("Edit", new { Id = Id, year = Model.Request.Year, requestTypeID = Model.Request.RequestTypeID });
            }

            var checkPreviousRequestInTheSameYear = myDB.Requests.FirstOrDefault(x => x.IsDeleted == false && x.Employee_UserName == Id && Model.Request.Year == x.Year);

            if (checkPreviousRequestInTheSameYear == null)
            {
                _RequestModel.Language = base.GetCurrentLanguage();

                string userName = GetUserName();

                ViewBag.WUserName = userName;
                ViewBag.FUserName = userName;

                if (_Permission.CheckCurrentUserIsManager(userName))
                {
                    return RedirectToAction("Evaluate", new { Id = Id, year = Model.Request.Year, requestTypeID = Model.Request.RequestTypeID });
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            else
            {
                Danger($"You cannot evaluate same employee in the same year. Request ID <strong>({checkPreviousRequestInTheSameYear.ID})</strong>, Year {Model.Request.Year}");
                return RedirectToAction("Draft", new { Id });
            }
        }

        #endregion

        #region Evaluate

        [HttpGet]
        public ActionResult Evaluate(string Id, int year, int requestTypeID)
        {
            _RequestModel.Language = base.GetCurrentLanguage();

            string userName = GetUserName();

            ViewBag.WUserName = userName;
            ViewBag.FUserName = userName;

            if (_Permission.CheckCurrentUserIsManager(userName))
            {
                EvaluateView(Id, userName, true, year, requestTypeID);

                _RequestModel.FormSubmit = true;
                _RequestModel.EditMode = "Evaluate";
                ViewBag.Title = "Evaluate";

                return View(_RequestModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public ActionResult Evaluate(string Id, RequestModel Model, HttpPostedFileBase File)
        {
            string userName = GetUserName();

            EvaluateView(ref Model, Id, userName);

            _RequestModel.EditMode = "Evaluate";
            ViewBag.Title = "Evaluate";

            var validation = Model.Request;

            List<Goal> goals = new List<Goal>();

            if (Model.FormSubmit)
            {
                Model.Request.RequestTypeID = Model.RequestTypeID;
                Model.Request.Year = Model.Year;
            }

            #region Validations

            if (Model.Grade == RequestTypeEnum.Supervisory || Model.Grade == RequestTypeEnum.NonSupervisory)
            {
                if (Model.Goals.Count > 0 && Model.Goals.Count < 3)
                {
                    Validation(Resource.GoalsValidation_InsertThreeAtLeast, nameof(validation.Goals), true);

                    return View(Model);
                }

                if (Model.Goals.Sum(x => x.RelativeWeight) != 100)
                {
                    Validation($"{Resource.GoalsName} - {Resource.RelativeWeightSumValidation}", nameof(validation.Goals), true);

                    return View(Model);
                }
            }

            if (Model.Competencies.Sum(x => x.RelativeWeight) != 100)
            {
                Validation($"{Resource.Competency} - {Resource.RelativeWeightSumValidation}", nameof(validation.Goals), true);

                return View(Model);
            }

            if (validation.Year == 0)
            {
                Validation(Resource.Year, nameof(validation.Year), true);
            }

            if (validation.RequestTypeID == null || validation.RequestTypeID == 0)
            {
                Validation(Resource.RequestType, nameof(validation.RequestTypeID), true);
            }

            #endregion

            #region Bind Goals

            if (Model.Competencies != null && Model.Competencies.Any())
            {
                foreach (var c in Model.Competencies)
                {
                    goals.Add(new Goal
                    {
                        Type = GoalTypeEnum.Competency.ToString(),
                        CompetencyLookupID = c.CompetencyLookupID,
                        NameAr = c.NameAr,
                        NameEn = c.NameEn,
                        RelativeWeight = c.RelativeWeight,
                    });
                }
            }

            if (Model.Goals != null && Model.Goals.Any())
            {
                foreach (var c in Model.Goals.Where(x => x.RelativeWeight > 0))
                {
                    goals.Add(new Goal
                    {
                        Type = GoalTypeEnum.Goal.ToString(),
                        Name = c.Name,
                        RelativeWeight = c.RelativeWeight,
                        Fulfillment = c.Fulfillment,
                        MeasurementStandard = c.MeasurementStandard,
                        TargetOutput = c.TargetOutput,
                    });
                }
            }
            #endregion

            //var HREmployess = _OasisSync.GetAllEmployeeByDeptCode(ConfigurationManager.AppSettings["Dept.HR"]);

            Employee _directManager = _OasisSync.GetEmployeeByUserName(userName);
            Employee _EmployeeDeptHead = _OasisSync.GetCurrentDepartmentHeadByDeptCode(_directManager.WORK_ENTITY);//For the Head
            Employee _employee = Model.SelectedEmployee;
            HRSAllowedUser hrManagerOfPersonal = myDB.HRSAllowedUsers.FirstOrDefault(x => x.PositionName == "ManagerOfPersonal");
            Employee _hr_ManagerOfPersonal = _OasisSync.GetEmployeeByUserName(hrManagerOfPersonal.UserName);
            Employee _hr_DirectorHead = _OasisSync.GetCurrentDepartmentHeadByDeptCode(ConfigurationManager.AppSettings["Dept.HR"]);//For the Head

            if (_EmployeeDeptHead == null && !string.IsNullOrEmpty(_directManager.DEPARTMENT_ENGLISH))
            {
                string deptCode = Regex.Match(_directManager.DEPARTMENT_ENGLISH, @"\d+").Value;

                if (!string.IsNullOrEmpty(deptCode))
                {
                    _EmployeeDeptHead = _OasisSync.GetCurrentDepartmentHeadByDeptCode(deptCode);//For the Head

                    if (_EmployeeDeptHead == null)
                    {
                        Validation(string.Format(Resource.DepartmentHeadNotFoundedPleaseContactWithHRTeam, _directManager.WORK_ENTITY, deptCode), nameof(validation.DeptHead_UserName), true);
                    }
                }
            }

            if (Model.Competencies.Any() &&
                ModelState.IsValid &&
                Model.Request.Year > 2000 &&
                _directManager != null &&
                _EmployeeDeptHead != null &&
                _employee != null &&
                _hr_ManagerOfPersonal != null &&
                _hr_DirectorHead != null
                )
            {
                var checkPreviousRequestInTheSameYear = myDB.Requests.FirstOrDefault(x => x.IsDeleted == false && x.Employee_UserName == Id && Model.Request.Year == x.Year);

                if (checkPreviousRequestInTheSameYear == null)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            #region Transaction-Scope
                            Model.Request.Goals = goals;

                            var lastDayOfTheYear = new DateTime(Model.Request.Year, 12, 31);

                            Model.Request.CreateDateTime = DateTime.Now;
                            Model.Request.DeadlineDate = lastDayOfTheYear;
                            Model.Request.StatusID = (int)RequestStatusEnum.RecievedByDepartmentHead;
                            Model.Request.RequestCounter = 1;

                            #region DirectManager
                            Model.Request.UserName = userName;
                            Model.Request.RequesterUserName = userName;
                            Model.Request.RequesterUserEnglishName = _directManager.ENGL_STAFF_NAME;
                            Model.Request.RequesterPositionCode = _directManager.POSITION_TYPE;
                            Model.Request.RequesterPositionName = _directManager.POSITION_ENGLISH;
                            Model.Request.RequesterDeptCode = _directManager.WORK_ENTITY;
                            Model.Request.RequesterDeptEnglishName = _directManager.DEPARTMENT_ENGLISH;
                            Model.Request.RequesterUserEmail = _directManager.EMAIL_ADDRESS;

                            Model.Request.DirectManager_UserName = _directManager.USER_NAME;
                            Model.Request.DirectManager_Email = _directManager.EMAIL_ADDRESS;
                            Model.Request.DirectManager_EnglishName = _directManager.ENGL_STAFF_NAME;
                            Model.Request.DirectManager_Position = _directManager.POSITION_ENGLISH;
                            #endregion

                            #region DepHead
                            Model.Request.DeptHead_UserName = _EmployeeDeptHead.DOC_NUMBER;
                            Model.Request.PendingToUserName = Model.Request.DeptHead_UserName;
                            Model.Request.DeptHead_EnglishName = _EmployeeDeptHead.ENGL_STAFF_NAME;
                            Model.Request.DeptHead_Email = _EmployeeDeptHead.EMAIL_ADDRESS;

                            Model.Request.DeptHead_Recieved = true;
                            Model.Request.DeptHead_RecievedDateTime = DateTime.Now;
                            #endregion

                            #region Evaluated -> Employee
                            Model.Request.Employee_UserName = _employee.DOC_NUMBER;
                            Model.Request.Employee_EnglishName = _employee.ENGL_STAFF_NAME;
                            Model.Request.Employee_Email = _employee.EMAIL_ADDRESS;
                            Model.Request.Employee_Position = _employee.POSITION_ENGLISH;
                            Model.Request.Employee_DepartmentEnglishName = _employee.DEPARTMENT_ENGLISH;
                            Model.Request.Employee_DeptCode = _employee.WORK_ENTITY;
                            Model.Request.Employee_StaffID = _employee.STAFF_ID;
                            #endregion

                            //#region PerformanceUnit Users

                            //#endregion

                            #region ManagerOfPersonal User
                            Model.Request.HR_ManagerOfPersonal_UserName = _hr_ManagerOfPersonal.DOC_NUMBER;
                            Model.Request.HR_ManagerOfPersonal_EnglishName = _hr_ManagerOfPersonal.ENGL_STAFF_NAME;
                            Model.Request.HR_ManagerOfPersonal_Email = _hr_ManagerOfPersonal.EMAIL_ADDRESS;
                            Model.Request.HR_ManagerOfPersonal_Position = _hr_ManagerOfPersonal.POSITION_ENGLISH;
                            Model.Request.HR_ManagerOfPersonal_DepartmentEnglishName = _hr_ManagerOfPersonal.DEPARTMENT_ENGLISH;
                            #endregion

                            #region HR_Head User 
                            Model.Request.HR_Director_UserName = _hr_DirectorHead.DOC_NUMBER;
                            Model.Request.HR_Director_EnglishName = _hr_DirectorHead.ENGL_STAFF_NAME;
                            Model.Request.HR_Director_Email = _hr_DirectorHead.EMAIL_ADDRESS;
                            Model.Request.HR_Director_Position = _hr_DirectorHead.POSITION_ENGLISH;
                            Model.Request.HR_Director_DepartmentEnglishName = _hr_DirectorHead.DEPARTMENT_ENGLISH;
                            #endregion

                            if (File != null && File.ContentLength > 0)
                            {
                                var FileName = Path.GetFileName(File.FileName);
                                string AttachmentPath = ConfigurationManager.AppSettings["AttachmentPath"] + Guid.NewGuid() + @"\";
                                if (!System.IO.Directory.Exists(AttachmentPath))
                                {
                                    _ = System.IO.Directory.CreateDirectory(AttachmentPath);
                                }
                                File.SaveAs(AttachmentPath + FileName);
                                Model.Request.Attachment = AttachmentPath + FileName;
                            }

                            _ = _RequestLogic.InsertRequest(Model.Request);

                            int RequestID = Model.Request.ID;

                            #endregion

                            SendCreateEmail(Model.Request, true);

                            myDB.SaveChanges();

                            scope.Complete();

                            Success(string.Format("New Request <strong>({0})</strong> was successfully created.", RequestID));

                            return RedirectToAction("Details", new { id = RequestID });
                        }
                        catch
                        {
                            Danger("Failed to create the request. Please check your data.");
                            return View("Evaluate", Model);
                        }
                    }
                }
                else
                {
                    Danger($"You cannot evaluate same employee in the same year. Request ID <strong>({checkPreviousRequestInTheSameYear.ID})</strong>, Year {Model.Request.Year}");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View("Evaluate", Model);
            }
        }

        #region Evaluate View Helpers
        private void EvaluateView(string Id, string userName, bool formSubmit, int year = 0, int requestTypeID = 0)
        {
            _RequestModel.SelectedEmployee = _OasisSync.GetEmployeeByUserName(Id);

            _RequestModel.MyEmployees = new List<Employee>();
            _RequestModel.StatusList = new List<Status>();
            _RequestModel.SActionList = new List<SAction>();
            _RequestModel.ReasonList = new List<Reason>();
            _RequestModel.Manager = new RequestManager();
            _RequestModel.Goals = new List<GoalViewModel>();
            _RequestModel.Competencies = new List<CompetencyViewModel>();
            _RequestModel.CurrentUserName = userName;
            _RequestModel.IsHRUser = _Permission.CheckUserIsHR(userName);

            _RequestModel.RequestTypes = myDB.RequestTypes.ToList();

            _RequestModel.FormSubmit = formSubmit == true && year > 2000 && requestTypeID > 0;

            if (_RequestModel.FormSubmit)
            {
                var requestType = (RequestTypeEnum)requestTypeID;

                _RequestModel.Grade = requestType;
                _RequestModel.Year = year;
                _RequestModel.RequestTypeID = requestTypeID;

                switch (requestType)
                {
                    case RequestTypeEnum.Supervisory:
                        _RequestModel.Goals.AddRange(GetGoals());
                        _RequestModel.Competencies.AddRange(GetCompetencies(false));
                        break;
                    case RequestTypeEnum.NonSupervisory:
                        _RequestModel.Goals.AddRange(GetGoals());
                        _RequestModel.Competencies.AddRange(GetCompetencies(true));
                        break;
                    case RequestTypeEnum.NonSupervisoryWithCompetenciesOnly:
                        _RequestModel.Competencies.AddRange(GetCompetencies(false));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var isUserPerformanceGuide = _RequestLogic.IsSupervisor(_RequestModel.SelectedEmployee.STAFF_ID);
                //isUserPerformanceGuide = true;
                if (isUserPerformanceGuide != null)
                {
                    if (isUserPerformanceGuide == true)
                    {
                        _RequestModel.Grade = RequestTypeEnum.Supervisory;
                        _RequestModel.Goals.AddRange(GetGoals());
                        _RequestModel.Competencies.AddRange(GetCompetencies(false));
                    }
                    else
                    {
                        _RequestModel.Grade = RequestTypeEnum.NonSupervisory;
                        _RequestModel.Goals.AddRange(GetGoals());
                        _RequestModel.Competencies.AddRange(GetCompetencies(true));
                    }
                }
                else
                {
                    string grade = _RequestModel.SelectedEmployee.GRADE_STEP;

                    if (int.TryParse(grade, out int gradeInt))
                    {
                        int first2Number = 0;

                        if (gradeInt.ToString().Count() > 1)
                        {
                            first2Number = gradeInt.ToString()[0] + gradeInt.ToString()[1];
                        }
                        else if (gradeInt.ToString().Count() == 1)
                        {
                            first2Number = gradeInt.ToString()[0];
                        }

                        if (first2Number > 13)
                        {
                            _RequestModel.Grade = RequestTypeEnum.Supervisory;
                            _RequestModel.Goals.AddRange(GetGoals());
                            _RequestModel.Competencies.AddRange(GetCompetencies(false));
                        }
                        else if (first2Number < 13)
                        {
                            _RequestModel.Grade = RequestTypeEnum.NonSupervisory;
                            _RequestModel.Goals.AddRange(GetGoals());
                            _RequestModel.Competencies.AddRange(GetCompetencies(true));
                        }
                        else if (first2Number < 6)
                        {
                            _RequestModel.Grade = RequestTypeEnum.NonSupervisoryWithCompetenciesOnly;
                            _RequestModel.Competencies.AddRange(GetCompetencies(false));
                        }
                        else
                        {
                            _RequestModel.Grade = RequestTypeEnum.unknown;
                        }
                    }
                    else
                    {
                        _RequestModel.Grade = RequestTypeEnum.unknown;
                    }
                }
            }
        }

        private bool EvaluateView(ref RequestModel model, string employeeUserName, string userName)
        {
            model.MyEmployees = new List<Employee>();
            model.StatusList = new List<Status>();
            model.SActionList = new List<SAction>();
            model.ReasonList = new List<Reason>();
            model.Manager = new RequestManager();

            Employee _employee = _OasisSync.GetEmployeeByUserName(employeeUserName);

            bool isEmpNull = _employee == null;

            model.SelectedEmployee = _employee;
            model.IsHRUser = _Permission.CheckUserIsHR(userName);
            model.CurrentUserName = userName;

            model.RequestTypes = myDB.RequestTypes.ToList();

            if (model.Grade == RequestTypeEnum.Supervisory)
            {
                model.Request.RequestTypeID = (int)RequestTypeEnum.Supervisory;
            }
            else if (model.Grade == RequestTypeEnum.NonSupervisory)
            {
                model.Request.RequestTypeID = (int)RequestTypeEnum.NonSupervisory;
            }
            else if (model.Grade == RequestTypeEnum.NonSupervisoryWithCompetenciesOnly)
            {
                model.Request.RequestTypeID = (int)RequestTypeEnum.NonSupervisoryWithCompetenciesOnly;
            }
            else
            {
                model.Request.RequestTypeID = (int)RequestTypeEnum.unknown;
            }

            return isEmpNull;
        }

        private List<GoalViewModel> GetGoals()
        {
            List<GoalViewModel> goalList = new List<GoalViewModel>();

            for (int i = 0; i < 6; i++)
            {
                goalList.Add(new GoalViewModel
                {
                    Index = i + 1
                });
            }

            return goalList;
        }

        private List<CompetencyViewModel> GetCompetencies(bool NonSupervisory)
        {
            List<CompetencyViewModel> competencyList = new List<CompetencyViewModel>();

            IQueryable<CompetencyLookup> com = null;

            if (NonSupervisory)
            {
                com = myDB.CompetencyLookups.Where(x => x.ID > 3).Include(x => x.CompetencyDetailsLookups);
            }
            else
            {
                com = myDB.CompetencyLookups.Include(x => x.CompetencyDetailsLookups);
            }

            foreach (var (c, i) in com.WithIndex())
            {
                var _g = new CompetencyViewModel
                {
                    Index = i + 1,
                    CompetencyLookupID = c.ID,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                };

                _g.Details = new List<CompetencyDetails>();

                foreach (var (d, _i) in c.CompetencyDetailsLookups.WithIndex())
                {
                    if (_RequestModel.Language == "Ar")
                    {
                        _g.Details.Add(new CompetencyDetails
                        {
                            Index = _i + 1,
                            CompetencyDesc = d.BehavioralDescriptionAr.Trim(),
                            CompetencyThemes = d.CompetencyThemesAr.Trim()
                        });
                    }
                    else
                    {
                        _g.Details.Add(new CompetencyDetails
                        {
                            Index = _i + 1,
                            CompetencyDesc = d.BehavioralDescriptionEn.Trim(),
                            CompetencyThemes = d.CompetencyThemesEn.Trim()
                        });
                    }
                }

                competencyList.Add(_g);
            }

            return competencyList;
        }

        #endregion

        private void SendCreateEmail(Request _Request, bool isCreate = false)
        {
            var _status = _StatusLogic.GetStatus(_Request.StatusID);
            var _type = myDB.RequestTypes.Find(_Request.RequestTypeID);

            string _toEmail = _Request.RequesterUserEmail;

            if (Environment.MachineName.Contains("MONIB"))
            {
                _toEmail = "mmonib@pscc.med.sa";
            }

            if (isCreate)
            {
                _Email.SendToNew("Create New Evaluation " + _type.NameEn, _toEmail, _Request.RequesterUserEnglishName, _Request.ID.ToString(), "", _Request.CreateDateTime.Value.ToString("dd/MM/yyyy"),
                     (_status != null) ? _status.Name : "");
            }

            _toEmail = _Request.DeptHead_Email;
            _Email.SendToApprove("Approved Evaluation " + _type.NameEn, _toEmail, "Department Director", _status, _Request);
        }

        private void Validation(string message, string propertyName = "", bool isWarning = false)
        {
            if (isWarning) Warning(message);

            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                ModelState.AddModelError($"Request.{propertyName}", message);
            }
        }

        #endregion

        #region Details
        //[EncryptedActionParameter]
        public ActionResult Details(int id, bool IsReport = false)
        {
            if (id > 0)
            {
                string Name = GetUserName();

                RequestModel requestModel = new RequestModel();

                if (_Permission.CheckUserIsRequestUserSingleRequest(Name, id)
                     || _Permission.CheckUserInDeptHeadSingleRequest(Name, id)
                     || _Permission.CheckUserIsHR(Name)
                     || _Permission.CheckUserIsRequestUserSingleRequest(Name, id))
                {
                    requestModel.Request = _RequestLogic.GetRequest(id);

                    requestModel.CurrentUserName = Name;
                    requestModel.IsHRUser = _Permission.CheckUserIsHR(Name);

                    if (requestModel.Request != null)
                    {
                        requestModel.Language = base.GetCurrentLanguage();

                        bool isEmpNull = EvaluateView(model: ref requestModel, requestModel.Request.Employee_UserName, Name);

                        if (isEmpNull == true)
                        {
                            Danger($"This employee is with ID {requestModel.Request.Employee_UserName} not founded in the system, please contact HR Team!");

                            return RedirectToAction("Index");
                        }
                        else if (requestModel.SelectedEmployee != null && requestModel.SelectedEmployee.CONTRACT_TYPE_ENG.ToLower().Contains("msd"))
                        {
                            Danger("This employee is MSD. Cannot evaluate MSD Employees!");
                            return RedirectToAction("Index");
                        }

                        requestModel.Goals = new List<GoalViewModel>();

                        foreach (var (g, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Goal.ToString()).WithIndex())
                        {
                            requestModel.Goals.Add(new GoalViewModel
                            {
                                Index = i + 1,
                                ID = g.ID,
                                Fulfillment = g.Fulfillment,
                                MeasurementStandard = g.MeasurementStandard,
                                Name = g.Name,
                                RelativeWeight = g.RelativeWeight,
                                RequestID = g.RequestID,
                                TargetOutput = g.TargetOutput
                            });
                        }

                        requestModel.Competencies = new List<CompetencyViewModel>();

                        foreach (var (c, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Competency.ToString()).WithIndex())
                        {
                            CompetencyViewModel item = new CompetencyViewModel
                            {
                                Index = i + 1,
                                ID = c.ID,
                                Fulfillment = c.Fulfillment,
                                MeasurementStandard = c.MeasurementStandard,
                                NameAr = c.NameAr,
                                NameEn = c.NameEn,
                                RelativeWeight = c.RelativeWeight,
                                RequestID = c.RequestID,
                                TargetOutput = c.TargetOutput,
                                CompetencyLookupID = c.CompetencyLookupID.Value,

                            };

                            item.Details = new List<CompetencyDetails>();

                            foreach (var (d, i2) in c.CompetencyLookup.CompetencyDetailsLookups.WithIndex())
                            {
                                if (requestModel.Language == "Ar")
                                {
                                    item.Details.Add(new CompetencyDetails
                                    {
                                        Index = i2 + 1,
                                        CompetencyDesc = d.BehavioralDescriptionAr.Trim(),
                                        CompetencyThemes = d.CompetencyThemesAr.Trim()
                                    });
                                }
                                else
                                {
                                    item.Details.Add(new CompetencyDetails
                                    {
                                        Index = i2 + 1,
                                        CompetencyDesc = d.BehavioralDescriptionEn.Trim(),
                                        CompetencyThemes = d.CompetencyThemesEn.Trim()
                                    });
                                }
                            }

                            requestModel.Competencies.Add(item);
                        }
                    }
                    else
                    {
                        Danger("No Request found.");
                        return RedirectToAction("Index");
                    }

                    return View(requestModel);
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

            }

            Danger("No Request found.");
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit & Track

        [HttpGet]
        public ActionResult Edit(int Id, int year, int requestTypeID)
        {
            try
            {
                string userName = GetUserName();

                if (_Permission.CheckUserIsRequestUserSingleRequest(userName, Id)
                     || _Permission.CheckUserInDeptHeadSingleRequest(userName, Id)
                     || _Permission.CheckUserIsHR(userName)
                     || _Permission.CheckUserIsRequestUserSingleRequest(userName, Id))
                {
                    RequestModel requestModel = new RequestModel();

                    var _request = _RequestLogic.GetRequest(Id);

                    if (_request != null)
                    {
                        ViewBag.WUserName = userName;
                        ViewBag.FUserName = userName;

                        requestModel.MyEmployees = new List<Employee>();
                        requestModel.StatusList = new List<Status>();
                        requestModel.SActionList = new List<SAction>();
                        requestModel.ReasonList = new List<Reason>();
                        requestModel.Manager = new RequestManager();

                        ViewBag.Title = "Edit";
                        requestModel.EditMode = "Edit";
                        requestModel.Request = _request;
                        requestModel.Language = base.GetCurrentLanguage();

                        requestModel.SelectedEmployee = _OasisSync.GetEmployeeByUserName(_request.Employee_UserName);
                        requestModel.RequestTypes = myDB.RequestTypes.ToList();

                        requestModel.CurrentUserName = userName;
                        requestModel.IsHRUser = _Permission.CheckUserIsHR(userName);
                        requestModel.IsFinished = _request.IsFinished;
                        requestModel.FormSubmit = true;
                        requestModel.Year = year;
                        requestModel.RequestTypeID = requestTypeID;
                        requestModel.Grade = (RequestTypeEnum)_request.RequestTypeID;

                        requestModel.Goals = new List<GoalViewModel>();

                        foreach (var (g, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Goal.ToString()).WithIndex())
                        {
                            requestModel.Goals.Add(new GoalViewModel
                            {
                                Index = i + 1,
                                ID = g.ID,
                                Fulfillment = g.Fulfillment,
                                MeasurementStandard = g.MeasurementStandard,
                                Name = g.Name,
                                RelativeWeight = g.RelativeWeight,
                                RequestID = g.RequestID,
                                TargetOutput = g.TargetOutput
                            });
                        }

                        requestModel.Competencies = new List<CompetencyViewModel>();

                        foreach (var (c, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Competency.ToString()).WithIndex())
                        {
                            CompetencyViewModel item = new CompetencyViewModel
                            {
                                Index = i + 1,
                                ID = c.ID,
                                Fulfillment = c.Fulfillment,
                                MeasurementStandard = c.MeasurementStandard,
                                NameAr = c.NameAr,
                                NameEn = c.NameEn,
                                RelativeWeight = c.RelativeWeight,
                                RequestID = c.RequestID,
                                TargetOutput = c.TargetOutput,
                                CompetencyLookupID = c.CompetencyLookupID.Value
                            };

                            item.Details = new List<CompetencyDetails>();

                            foreach (var (d, i2) in c.CompetencyLookup.CompetencyDetailsLookups.WithIndex())
                            {
                                if (requestModel.Language == "Ar")
                                {
                                    item.Details.Add(new CompetencyDetails
                                    {
                                        Index = i2 + 1,
                                        CompetencyDesc = d.BehavioralDescriptionAr.Trim(),
                                        CompetencyThemes = d.CompetencyThemesAr.Trim()
                                    });
                                }
                                else
                                {
                                    item.Details.Add(new CompetencyDetails
                                    {
                                        Index = i2 + 1,
                                        CompetencyDesc = d.BehavioralDescriptionEn.Trim(),
                                        CompetencyThemes = d.CompetencyThemesEn.Trim()
                                    });
                                }
                            }

                            requestModel.Competencies.Add(item);
                        }

                        return View("Evaluate", requestModel);
                    }
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            catch
            {
            }

            Danger("No Request found.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int Id, RequestModel Model, HttpPostedFileBase File)
        {
            string userName = GetUserName();

            if (_Permission.CheckUserIsRequestUserSingleRequest(userName, Id)
                 || _Permission.CheckUserInDeptHeadSingleRequest(userName, Id)
                 || _Permission.CheckUserIsHR(userName)
                 || _Permission.CheckUserIsRequestUserSingleRequest(userName, Id))
            {
                Request _request = _RequestLogic.GetRequest(Id);

                if (_request != null)
                {
                    EvaluateView(ref Model, _request.Employee_UserName, userName);

                    var validation = Model.Request;

                    List<Goal> goals = new List<Goal>();

                    #region Validations

                    if (Model.Grade == RequestTypeEnum.Supervisory || Model.Grade == RequestTypeEnum.NonSupervisory)
                    {
                        if (Model.Goals.Count > 0 && Model.Goals.Count < 3)
                        {
                            Validation(Resource.GoalsValidation_InsertThreeAtLeast, nameof(validation.Goals), true);

                            return View(Model);
                        }

                        if (Model.Goals.Sum(x => x.RelativeWeight) != 100)
                        {
                            Validation($"{Resource.GoalsName} - {Resource.RelativeWeightSumValidation}", nameof(validation.Goals), true);

                            return View(Model);
                        }
                    }

                    if (Model.Competencies.Sum(x => x.RelativeWeight) != 100)
                    {
                        Validation($"{Resource.Competency} - {Resource.RelativeWeightSumValidation}", nameof(validation.Goals), true);

                        return View(Model);
                    }

                    if (validation.RequestTypeID == 0)
                    {
                        Validation(Resource.RequestType, nameof(validation.RequestTypeID), true);
                    }

                    #endregion

                    #region Bind Goals

                    if (Model.Competencies != null && Model.Competencies.Any())
                    {
                        foreach (var c in Model.Competencies)
                        {
                            goals.Add(new Goal
                            {
                                Type = GoalTypeEnum.Competency.ToString(),
                                CompetencyLookupID = c.CompetencyLookupID,
                                NameAr = c.NameAr,
                                NameEn = c.NameEn,
                                RelativeWeight = c.RelativeWeight,
                            });
                        }
                    }

                    if (Model.Goals != null && Model.Goals.Any())
                    {
                        foreach (var c in Model.Goals.Where(x => x.RelativeWeight > 0))
                        {
                            goals.Add(new Goal
                            {
                                Type = GoalTypeEnum.Goal.ToString(),
                                Name = c.Name,
                                RelativeWeight = c.RelativeWeight,
                                Fulfillment = c.Fulfillment,
                                MeasurementStandard = c.MeasurementStandard,
                                TargetOutput = c.TargetOutput,
                            });
                        }
                    }
                    #endregion

                    if (Model.Competencies.Any() && ModelState.IsValid)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                #region Transaction-Scope
                                var _requestGoals = myDB.Goals.Where(x => x.RequestID == _request.ID);
                                myDB.Goals.RemoveRange(_requestGoals);
                                myDB.SaveChanges();

                                foreach (var g in goals)
                                {
                                    _request.Goals.Add(g);
                                }
                                myDB.SaveChanges();

                                _request.Year = Model.Year;
                                _request.RequestTypeID = Model.RequestTypeID;
                                _request.StatusID = (int)RequestStatusEnum.RecievedByDepartmentHead;
                                _request.Note = Model.Request.Note;
                                _request.PendingToUserName = Model.Request.DeptHead_UserName;

                                if (File != null && File.ContentLength > 0)
                                {
                                    var FileName = Path.GetFileName(File.FileName);
                                    string AttachmentPath = ConfigurationManager.AppSettings["AttachmentPath"] + Guid.NewGuid() + @"\";
                                    if (!System.IO.Directory.Exists(AttachmentPath))
                                    {
                                        _ = System.IO.Directory.CreateDirectory(AttachmentPath);
                                    }
                                    File.SaveAs(AttachmentPath + FileName);
                                    _request.Attachment = AttachmentPath + FileName;
                                }

                                _ = _RequestLogic.UpdateRequest(_request);

                                int RequestID = _request.ID;

                                #endregion

                                SendCreateEmail(_request);

                                myDB.SaveChanges();

                                scope.Complete();

                                Success(string.Format("Request <strong>({0})</strong> was updated successfully.", RequestID));

                                return RedirectToAction("Details", new { id = RequestID });
                            }
                            catch
                            {
                                Danger("Failed to create the request. Please check your data.");
                                return View("Evaluate", Model);
                            }
                        }
                    }
                    else
                    {
                        Danger("Failed to create the request. Please check your data.");
                        return View("Evaluate", Model);
                    }
                }

                Danger("No Request found.");

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        public ActionResult Track(int id)
        {
            if (id > 0)
            {
                string Name = GetUserName();

                if (_Permission.CheckUserIsRequestUserSingleRequest(Name, id)
                     || _Permission.CheckUserInDeptHeadSingleRequest(Name, id)
                     || _Permission.CheckUserIsHR(Name)
                     || _Permission.CheckUserIsRequestUserSingleRequest(Name, id))
                {

                    _RequestModel.Request = _RequestLogic.GetRequest(id);

                    _RequestModel.CurrentUserName = Name;
                    _RequestModel.IsHRUser = _Permission.CheckUserIsHR(Name);

                    if (_RequestModel.Request != null)
                    {
                        _RequestModel.Language = base.GetCurrentLanguage();

                        return View(_RequestModel);
                    }
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            Danger("No Request found.");
            return RedirectToAction("Index");
        }

        #endregion

        #region Manage & Actions

        #region Manage
        //[EncryptedActionParameter]
        public ActionResult Manage(int id)
        {
            string userName = GetUserName();

            if (_RequestLogic.CheckUserAccessToRequest(id, userName) ||
                _Permission.CheckUserIsHR(userName))
            {
                RequestModel requestModel = new RequestModel();

                requestModel.Request = _RequestLogic.GetRequest(id);

                requestModel.CurrentUserName = userName;
                requestModel.IsHRUser = _Permission.CheckUserIsHR(userName);
                requestModel.Language = base.GetCurrentLanguage();

                if (requestModel.Request != null)
                {
                    bool isEmpNull = EvaluateView(model: ref requestModel, requestModel.Request.Employee_UserName, userName);

                    if (isEmpNull == true)
                    {
                        Danger($"This employee is with ID {requestModel.Request.Employee_UserName} not founded in the system, please contact HR Team!");

                        return RedirectToAction("Index");
                    }
                    else if (requestModel.SelectedEmployee != null && requestModel.SelectedEmployee.CONTRACT_TYPE_ENG.ToLower().Contains("msd"))
                    {
                        Danger("This employee is MSD. Cannot evaluate MSD Employees!");
                        return RedirectToAction("Index");
                    }

                    #region Goals
                    requestModel.Goals = new List<GoalViewModel>();

                    foreach (var (g, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Goal.ToString()).WithIndex())
                    {
                        requestModel.Goals.Add(new GoalViewModel
                        {
                            Index = i + 1,
                            ID = g.ID,
                            Fulfillment = g.Fulfillment,
                            MeasurementStandard = g.MeasurementStandard,
                            Name = g.Name,
                            RelativeWeight = g.RelativeWeight,
                            RequestID = g.RequestID,
                            TargetOutput = g.TargetOutput
                        });
                    }

                    requestModel.Competencies = new List<CompetencyViewModel>();

                    foreach (var (c, i) in requestModel.Request.Goals.Where(x => x.Type == GoalTypeEnum.Competency.ToString()).WithIndex())
                    {
                        CompetencyViewModel item = new CompetencyViewModel
                        {
                            Index = i + 1,
                            ID = c.ID,
                            Fulfillment = c.Fulfillment,
                            MeasurementStandard = c.MeasurementStandard,
                            NameAr = c.NameAr,
                            NameEn = c.NameEn,
                            RelativeWeight = c.RelativeWeight,
                            RequestID = c.RequestID,
                            TargetOutput = c.TargetOutput,
                            CompetencyLookupID = c.CompetencyLookupID.Value,

                        };

                        item.Details = new List<CompetencyDetails>();

                        foreach (var (d, i2) in c.CompetencyLookup.CompetencyDetailsLookups.WithIndex())
                        {
                            if (requestModel.Language == "Ar")
                            {
                                item.Details.Add(new CompetencyDetails
                                {
                                    Index = i2 + 1,
                                    CompetencyDesc = d.BehavioralDescriptionAr.Trim(),
                                    CompetencyThemes = d.CompetencyThemesAr.Trim()
                                });
                            }
                            else
                            {
                                item.Details.Add(new CompetencyDetails
                                {
                                    Index = i2 + 1,
                                    CompetencyDesc = d.BehavioralDescriptionEn.Trim(),
                                    CompetencyThemes = d.CompetencyThemesEn.Trim()
                                });
                            }
                        }

                        requestModel.Competencies.Add(item);
                    }
                    #endregion

                    #region Validations
                    requestModel.IsPerformanceUnitUser = _RequestLogic.IsPerformanceUntiUser(userName);
                    #endregion

                    requestModel.SActionList = _SActionLogic.GetAllSAction();
                    requestModel.ReasonList = _ReasonLogic.GetAllReason().ToList();

                    requestModel.DeptHead_UserName = requestModel.Request.DeptHead_UserName;
                    requestModel.Employee_UserName = requestModel.Request.Employee_UserName;
                    requestModel.DirectManager_UserName = requestModel.Request.DirectManager_UserName;
                    requestModel.HR_ManagerOfPersonal_UserName = requestModel.Request.HR_ManagerOfPersonal_UserName;
                    requestModel.HR_Director_UserName = requestModel.Request.HR_Director_UserName;
                }
                else
                {
                    Danger("No Request found.");
                    return RedirectToAction("Index");
                }

                return View(requestModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        #endregion

        #region Steps/Actions 

        [HttpPost]
        public ActionResult DepartmentHead_Action(RequestModel Model)
        {
            try
            {
                string currentUsername = GetUserName();

                if (ModelState.IsValid && Model.DeptHead_UserName == currentUsername)
                {
                    Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    _Request.DeptHead_Approval = Model.Request.DeptHead_Approval;
                    _Request.DeptHead_ApprovalDateTime = DateTime.Now;

                    _Request.DeptHead_Seen = true;
                    _Request.DeptHead_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.DeptHead_UserName;

                    if (Model.Request.DeptHead_Approval == true)
                    {
                        string _toEmail, sendToAprrove;

                        _Request.StatusID = (int)RequestStatusEnum.RecievedByEmployee;
                        _Request.Employee_Recieved = true;
                        _Request.Employee_RecievedDateTime = DateTime.Now;
                        _toEmail = _Request.DirectManager_Email;
                        sendToAprrove = _Request.Employee_Position;
                        _Request.PendingToUserName = _Request.Employee_UserName;

                        if (Environment.MachineName.Contains("MONIB"))
                        {
                            _toEmail = "mmonib@pscc.med.sa";
                        }

                        _Email.SendToApprove(_Request.Status.Name, _toEmail, sendToAprrove, _Request.Status, _Request);
                    }
                    else if (Model.Request.DeptHead_Approval == false)
                    {
                        string _toEmail = "", _position = "";

                        _Request.DeptHead_Reason = Model.Request.DeptHead_Reason;
                        _Request.RequestCounter++;

                        //Next Step DirectManager
                        _Request.DirectManager_Recieved = true;
                        _Request.DirectManager_RecievedDateTime = DateTime.Now;
                        _Request.PendingToUserName = _Request.DirectManager_UserName;
                        //Next Step DirectManager

                        _Request.StatusID = (int)RequestStatusEnum.RejectedByDepartmentHead;
                        _Request.DeptHead_Approval = false;
                        _Request.RejectedBy = RequestStatusEnum.RejectedByDepartmentHead.ToString();
                        _Request.PendingToUserName = _Request.DirectManager_UserName;

                        _toEmail = _Request.DirectManager_Email;
                        _position = _Request.DirectManager_Position;

                        if (Environment.MachineName.Contains("MONIB"))
                        {
                            _toEmail = "mmonib@pscc.med.sa";
                        }

                        _Email.SendToApprove(_Request.Status.Name, _toEmail, _position, _Request.Status, _Request);
                    }

                    myDB.Entry(_Request).State = EntityState.Modified;
                    _ = myDB.SaveChanges();

                    Success(string.Format("Department Head Action For Request <strong>({0})</strong> has been updated successfully.", _Request.ID));
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Manage", new { id = Model.Request.ID });
        }

        [HttpPost]
        public ActionResult Employee_Action(RequestModel Model)
        {
            try
            {
                string currentUsername = GetUserName();

                if (ModelState.IsValid && Model.Employee_UserName == currentUsername)
                {
                    Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    _Request.Employee_ActionID = Model.Request.Employee_ActionID;
                    _Request.Employee_ActionDateTime = DateTime.Now;
                    _Request.Employee_ReasonID = Model.Request.Employee_ReasonID;

                    _Request.Employee_Comment = Model.Request.Employee_Comment;

                    _Request.Employee_ApprovalDateTime = DateTime.Now;

                    _Request.Employee_Seen = true;
                    _Request.Employee_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.Employee_UserName;

                    if (Model.Request.Employee_ActionID != null)
                    {
                        string _toEmail = "", _position = "";

                        switch (Model.Request.Employee_ActionID)
                        {
                            case (int)ActionEnum.Resolved:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.RecievedByHRPerformanceUnit;
                                    _Request.Employee_Approval = true;

                                    //Next Step _HRPerformanceUnitAction
                                    _Request.HR_PerformanceUnit_Recieved = true;
                                    _Request.HR_PerformanceUnit_RecievedDateTime = DateTime.Now;
                                    _Request.PendingToUserName = _Request.HR_PerformanceUnit_UserName;
                                    //Next Step _HRPerformanceUnitAction

                                    _toEmail = _Request.HR_PerformanceUnit_Email;
                                    _position = _Request.HR_PerformanceUnit_Position;

                                    if (Environment.MachineName.Contains("MONIB"))
                                    {
                                        _toEmail = "mmonib@pscc.med.sa";
                                    }

                                    break;
                                }
                            case (int)ActionEnum.Reject:
                                {
                                    _Request.RequestCounter++;
                                    _Request.Employee_Approval = false;
                                    _Request.Employee_Reason = Model.Request.Employee_Reason;

                                    _Request.StatusID = (int)RequestEditableStatusEnum.RejectedByEmployeeFirstTime;
                                    _Request.RejectedBy = RequestEditableStatusEnum.RejectedByEmployeeFirstTime.ToString();
                                    _Request.IsRejectedByEmpFirstTime = true;

                                    if (_Request.IsRejectedByEmpFirstTime)
                                    {
                                        _Request.StatusID = (int)RequestEditableStatusEnum.RejectedByEmployeeSecondTime;
                                        _Request.RejectedBy = RequestEditableStatusEnum.RejectedByEmployeeSecondTime.ToString();
                                        _Request.IsRejectedByEmpSecondTime = true;
                                    }

                                    if (_Request.IsRejectedByEmpFirstTime && _Request.IsRejectedByEmpSecondTime)
                                    {
                                        //Next Step _HRPerformanceUnitAction
                                        _Request.HR_PerformanceUnit_Recieved = true;
                                        _Request.HR_PerformanceUnit_RecievedDateTime = DateTime.Now;
                                        _Request.PendingToUserName = _Request.HR_PerformanceUnit_UserName;
                                        //Next Step _HRPerformanceUnitAction

                                        _toEmail = _Request.HR_PerformanceUnit_Email;
                                        _position = _Request.HR_PerformanceUnit_Position;

                                        if (Environment.MachineName.Contains("MONIB"))
                                        {
                                            _toEmail = "mmonib@pscc.med.sa";
                                        }
                                    }
                                    else
                                    {
                                        //Next Step DirectManager
                                        _Request.DirectManager_Recieved = true;
                                        _Request.DirectManager_RecievedDateTime = DateTime.Now;
                                        _Request.PendingToUserName = _Request.DirectManager_UserName;
                                        //Next Step DirectManager

                                        _toEmail = _Request.DirectManager_Email;
                                        _position = _Request.DirectManager_Position;

                                        if (Environment.MachineName.Contains("MONIB"))
                                        {
                                            _toEmail = "mmonib@pscc.med.sa";
                                        }
                                    }

                                    break;
                                }
                            case (int)ActionEnum.UnderProcessing:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.UnderProcessingByEmployee;
                                    _Request.Employee_Approval = false;
                                    break;
                                }
                        }

                        if (!string.IsNullOrEmpty(_toEmail) && !string.IsNullOrEmpty(_position))
                        {
                            _Email.SendToApprove(_Request.Status.Name, _toEmail, _position, _Request.Status, _Request);
                        }
                    }

                    myDB.Entry(_Request).State = EntityState.Modified;

                    _ = myDB.SaveChanges();

                    Success(string.Format("Employee Action For Request <strong>({0})</strong> has been updated successfully.", Model.Request.ID));
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                    return RedirectToAction("Manage", new { id = Model.Request.ID });
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult HR_PerformanceUnit_Action(RequestModel Model)
        {
            try
            {
                if (ModelState.IsValid && Model.IsPerformanceUnitUser == true)
                {
                    Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    Employee _Employee = _OasisSync.GetEmployeeByUserName(GetUserName());

                    _Request.HR_PerformanceUnit_UserName = GetUserName();
                    _Request.HR_PerformanceUnit_Email = _Employee.EMAIL_ADDRESS;
                    _Request.HR_PerformanceUnit_EnglishName = _Employee.ENGL_STAFF_NAME;
                    _Request.HR_PerformanceUnit_Position = _Employee.POSITION_ENGLISH;

                    _Request.HR_PerformanceUnit_ActionID = Model.Request.HR_PerformanceUnit_ActionID;
                    _Request.HR_PerformanceUnit_ActionDateTime = DateTime.Now;
                    _Request.HR_PerformanceUnit_ReasonID = Model.Request.HR_PerformanceUnit_ReasonID;

                    _Request.HR_PerformanceUnit_Comment = Model.Request.HR_PerformanceUnit_Comment;

                    _Request.HR_PerformanceUnit_ApprovalDateTime = DateTime.Now;

                    _Request.HR_PerformanceUnit_Seen = true;
                    _Request.HR_PerformanceUnit_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.HR_PerformanceUnit_UserName;

                    if (Model.Request.HR_PerformanceUnit_ActionID != null)
                    {
                        string _toEmail = "", _position = "";

                        switch (Model.Request.HR_PerformanceUnit_ActionID)
                        {
                            case (int)ActionEnum.Resolved:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.RecievedByHRManagerOfPersonnel;
                                    _Request.HR_PerformanceUnit_Approval = true;

                                    //Next Step HR_ManagerOfPersonal
                                    _Request.HR_ManagerOfPersonal_Recieved = true;
                                    _Request.HR_ManagerOfPersonal_RecievedDateTime = DateTime.Now;
                                    _Request.PendingToUserName = _Request.HR_ManagerOfPersonal_UserName;
                                    //Next Step HR_ManagerOfPersonal

                                    _toEmail = _Request.HR_ManagerOfPersonal_Email;
                                    _position = _Request.HR_ManagerOfPersonal_Position;

                                    if (Environment.MachineName.Contains("MONIB"))
                                    {
                                        _toEmail = "mmonib@pscc.med.sa";
                                    }

                                    break;
                                }
                            case (int)ActionEnum.Reject:
                                {
                                    _Request.RequestCounter++;

                                    //Next Step DirectManager
                                    _Request.DirectManager_Recieved = true;
                                    _Request.DirectManager_RecievedDateTime = DateTime.Now;
                                    _Request.PendingToUserName = _Request.DirectManager_UserName;
                                    //Next Step DirectManager

                                    _Request.StatusID = (int)RequestEditableStatusEnum.RejectedByHRPerformanceUnit;
                                    _Request.RejectedBy = RequestEditableStatusEnum.RejectedByHRPerformanceUnit.ToString();
                                    _Request.HR_PerformanceUnit_Reason = Model.Request.HR_PerformanceUnit_Reason;
                                    _Request.HR_PerformanceUnit_Approval = false;
                                    _Request.PendingToUserName = _Request.DirectManager_UserName;

                                    _toEmail = _Request.DirectManager_Email;
                                    _position = _Request.DirectManager_Position;

                                    if (Environment.MachineName.Contains("MONIB"))
                                    {
                                        _toEmail = "mmonib@pscc.med.sa";
                                    }

                                    break;
                                }
                            case (int)ActionEnum.UnderProcessing:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.UnderProcessingByHRManagerOfPersonnel;
                                    _Request.HR_PerformanceUnit_Approval = false;
                                    break;
                                }
                        }

                        if (!string.IsNullOrEmpty(_toEmail) && !string.IsNullOrEmpty(_position))
                        {
                            _Email.SendToApprove(_Request.Status.Name, _toEmail, _position, _Request.Status, _Request);
                        }
                    }

                    myDB.Entry(_Request).State = EntityState.Modified;

                    _ = myDB.SaveChanges();

                    Success(string.Format("Associate Dept Rep Action For Request <strong>({0})</strong> has been updated successfully.", Model.Request.ID));
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                    return RedirectToAction("Manage", new { id = Model.Request.ID });
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DirectManager_Action(RequestModel Model)
        {
            try
            {
                string currentUsername = GetUserName();

                Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                if (ModelState.IsValid && _Request != null && _Request.DirectManager_UserName == currentUsername)
                {
                    _Request.DirectManager_ActionID = Model.Request.DirectManager_ActionID;
                    _Request.DirectManager_ActionDateTime = DateTime.Now;
                    _Request.DirectManager_ReasonID = Model.Request.DirectManager_ReasonID;

                    _Request.DirectManager_Comment = Model.Request.DirectManager_Comment;

                    _Request.DirectManager_ApprovalDateTime = DateTime.Now;

                    _Request.DirectManager_Seen = true;
                    _Request.DirectManager_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.DirectManager_UserName;

                    string _toEmail = "";

                    _Request.StatusID = (int)RequestStatusEnum.RecievedByDepartmentHead;
                    _Request.DirectManager_Approval = true;

                    Model.Request.DirectManager_ActionID = 1;
                    Model.Request.DirectManager_Approval = true;
                    Model.Request.DirectManager_ApprovalDateTime = DateTime.Now;

                    //Next Step _HRAction
                    _Request.DeptHead_Recieved = true;
                    _Request.DeptHead_RecievedDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.DeptHead_UserName;
                    _Request.RejectedBy = null;
                    //Next Step _HRAction

                    _toEmail = _Request.DeptHead_Email;

                    if (Environment.MachineName.Contains("MONIB"))
                    {
                        _toEmail = "mmonib@pscc.med.sa";
                    }

                    _Email.SendToApprove(_Request.Status?.Name, _toEmail, _Request.DirectManager_Position, _Request.Status, _Request);

                    myDB.Entry(_Request).State = EntityState.Modified;

                    _ = myDB.SaveChanges();

                    Success(string.Format("Direct Manager Action For Request <strong>({0})</strong> has been updated successfully.", Model.Request.ID));

                    return RedirectToAction("Draft", new { Id = Model.Request.ID });
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                    return RedirectToAction("Manage", new { id = Model.Request.ID });
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult HR_ManagerOfPersonal_Action(RequestModel Model)
        {
            try
            {
                string currentUsername = GetUserName();

                if (ModelState.IsValid && Model.HR_ManagerOfPersonal_UserName == currentUsername)
                {
                    Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    _Request.HR_ManagerOfPersonal_ActionID = Model.Request.HR_ManagerOfPersonal_ActionID;
                    _Request.HR_ManagerOfPersonal_ActionDateTime = DateTime.Now;
                    _Request.HR_ManagerOfPersonal_ReasonID = Model.Request.HR_ManagerOfPersonal_ReasonID;

                    _Request.HR_ManagerOfPersonal_Comment = Model.Request.HR_ManagerOfPersonal_Comment;

                    _Request.HR_ManagerOfPersonal_ApprovalDateTime = DateTime.Now;

                    _Request.HR_ManagerOfPersonal_Seen = true;
                    _Request.HR_ManagerOfPersonal_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.HR_ManagerOfPersonal_UserName;

                    if (Model.Request.HR_ManagerOfPersonal_ActionID != null)
                    {
                        string _toEmail = "";

                        switch (Model.Request.HR_ManagerOfPersonal_ActionID)
                        {
                            case (int)ActionEnum.Resolved:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.RecievedByHRHead;
                                    _Request.HR_ManagerOfPersonal_Approval = true;

                                    //Next Step HR_Director
                                    _Request.HR_Director_Recieved = true;
                                    _Request.HR_Director_RecievedDateTime = DateTime.Now;
                                    _Request.PendingToUserName = _Request.HR_Director_UserName;
                                    //Next Step HR_Director

                                    _toEmail = _Request.HR_ManagerOfPersonal_Email;
                                    if (Environment.MachineName.Contains("MONIB"))
                                    {
                                        _toEmail = "mmonib@pscc.med.sa";
                                    }

                                    _Email.SendToApprove(_Request.Status?.Name, _toEmail, _Request.HR_ManagerOfPersonal_Position, _Request.Status, _Request);

                                    break;
                                }
                            case (int)ActionEnum.Reject:
                                {
                                    string _position = "";

                                    //Next Step DirectManager
                                    _Request.DirectManager_Recieved = true;
                                    _Request.DirectManager_RecievedDateTime = DateTime.Now;
                                    _Request.PendingToUserName = _Request.DirectManager_UserName;
                                    //Next Step DirectManager

                                    _Request.RequestCounter++;
                                    _Request.HR_ManagerOfPersonal_Approval = false;
                                    _Request.HR_ManagerOfPersonal_Reason = Model.Request.HR_ManagerOfPersonal_Reason;
                                    _Request.StatusID = (int)RequestStatusEnum.RejectedByHRManagerOfPersonnel;
                                    _Request.RejectedBy = RequestStatusEnum.RejectedByHRManagerOfPersonnel.ToString();

                                    _toEmail = _Request.DirectManager_Email;
                                    _position = _Request.DirectManager_Position;

                                    if (Environment.MachineName.Contains("MONIB"))
                                    {
                                        _toEmail = "mmonib@pscc.med.sa";
                                    }

                                    _Email.SendToApprove(_Request.Status.Name, _toEmail, _position, _Request.Status, _Request);

                                    break;
                                }
                            case (int)ActionEnum.UnderProcessing:
                                {
                                    _Request.StatusID = (int)RequestStatusEnum.UnderProcessingByHRManagerOfPersonnel;
                                    _Request.HR_ManagerOfPersonal_Approval = false;
                                    break;
                                }
                        }
                    }

                    myDB.Entry(_Request).State = EntityState.Modified;

                    _ = myDB.SaveChanges();

                    Success(string.Format("PSCC Supplies Assistant Director Action For Request <strong>({0})</strong> has been updated successfully.", Model.Request.ID));
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                    return RedirectToAction("Manage", new { id = Model.Request.ID });
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult HR_Director_Action(RequestModel Model)
        {
            try
            {
                string currentUsername = GetUserName();

                if (ModelState.IsValid && Model.HR_Director_UserName == currentUsername)
                {
                    Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    _Request.HR_Director_ActionID = Model.Request.HR_Director_ActionID;
                    _Request.HR_Director_ActionDateTime = DateTime.Now;
                    _Request.HR_Director_ReasonID = Model.Request.HR_Director_ReasonID;

                    _Request.HR_Director_Comment = Model.Request.HR_Director_Comment;

                    _Request.HR_Director_ApprovalDateTime = DateTime.Now;

                    _Request.HR_Director_Seen = true;
                    _Request.HR_Director_SeenDateTime = DateTime.Now;
                    _Request.PendingToUserName = _Request.HR_Director_UserName;

                    string _toEmail = "";

                    if (Model.Request.HR_Director_Approval == true)
                    {
                        _Request.StatusID = (int)RequestStatusEnum.Finished;

                        //Next Step Last
                        _Request.HR_Director_Approval = true;
                        _Request.IsFinished = true;
                        //Next Step Last

                        _toEmail = _Request.RequesterUserEmail;

                        if (Environment.MachineName.Contains("MONIB"))
                        {
                            _toEmail = "mmonib@pscc.med.sa";
                        }

                        _Email.SendToClosedReject(_Request.Status?.Name, _toEmail, _Request.RequesterUserEnglishName, _Request.ID.ToString(), "Resolved ", "إنهاء", "", _Request.CreateDateTime.Value.ToString("dd/MM/yyyy"),
                            base.GetCurrentLanguage() == "Ar" ? _Request.Status.NameAr : _Request.Status.Name, _Request.Note);
                    }
                    else
                    {
                        string _position = "";

                        //Next Step DirectManager
                        _Request.DirectManager_Recieved = true;
                        _Request.DirectManager_RecievedDateTime = DateTime.Now;
                        _Request.PendingToUserName = _Request.DirectManager_UserName;
                        //Next Step DirectManager

                        _Request.RequestCounter++;
                        _Request.IsFinished = false;
                        _Request.HR_Director_Approval = false;
                        _Request.HR_Director_Reason = Model.Request.HR_Director_Reason;
                        _Request.StatusID = (int)RequestStatusEnum.RejectedByHRManagerOfPersonnel;
                        _Request.RejectedBy = RequestStatusEnum.RejectedByHRManagerOfPersonnel.ToString();

                        _toEmail = _Request.DirectManager_Email;
                        _position = _Request.DirectManager_Position;

                        if (Environment.MachineName.Contains("MONIB"))
                        {
                            _toEmail = "mmonib@pscc.med.sa";
                        }

                        _Email.SendToApprove(_Request.Status.Name, _toEmail, _position, _Request.Status, _Request);
                    }

                    myDB.Entry(_Request).State = EntityState.Modified;

                    _ = myDB.SaveChanges();

                    Success(string.Format("HR Director Final Approval Action For Request <strong>({0})</strong> has been updated successfully.", Model.Request.ID));
                }
                else
                {
                    Danger(Resource.AccessPermissionToThisAction);
                    return RedirectToAction("Manage", new { id = Model.Request.ID });
                }
            }
            catch
            {
                Danger("Failed to update. Please check your data.");
            }

            return RedirectToAction("Manage", new { id = Model.Request.ID });
        }

        #endregion

        #endregion

        #region GiveFeedBack

        [HttpPost]
        public ActionResult GiveFeedBack(RequestModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DML.MainEntities.Request _Request = myDB.Requests.Include(x => x.Status).FirstOrDefault(x => x.ID == Model.Request.ID);

                    _Request.FeedBackGiven = true;
                    _Request.FeedBackGivenDateTime = DateTime.Now;

                    _Request.AchievementFeedBack = (Model.Request.AchievementFeedBack != null) ? Model.Request.AchievementFeedBack : 0;
                    _Request.TimeFeedBack = (Model.Request.TimeFeedBack != null) ? Model.Request.TimeFeedBack : 0;
                    _Request.SatisfacationFeedBack = (Model.Request.SatisfacationFeedBack != null) ? Model.Request.SatisfacationFeedBack : 0;

                    ////for the demand
                    //string message = @"Your Feedback For The Request <a href='" + baseUrl + "/Request/Details/" + _Request.ID + "'> " + _Request.ID + "</a> has been sent.";
                    //string Subject = "Feedback Sent";
                    //string _toEmail = _Request.RequestUserUserName + "@pscc.med.sa";
                    //_Email.SendTo(message, _toEmail, Subject);

                    ////for the form HRapprover
                    //message = @"New Feedback For The Request <a href='" + baseUrl + "/Request/Details/" + _Request.ID + "'> " + _Request.ID + "</a> has been sent by <b>(" + _Request.RequesterUserEnglishName + ")</b>, in <b>(" + _Request.RequesterDeptEnglishName + ") </b>Department.";
                    //Subject = "New Feedback Sent";
                    //_toEmail = _Request.ApproverUserName + "@pscc.med.sa";
                    //_Email.SendTo(message, _toEmail, Subject);

                    ////for the form admin
                    //message = @"New Feedback For The Request <a href='" + baseUrl + "/Request/Details/" + _Request.ID + "'> " + _Request.ID + "</a> has been sent by <b>(" + _Request.RequesterUserEnglishName + ")</b>, in <b>(" + _Request.RequesterDeptEnglishName + ") </b>Department.";
                    //Subject = "New Feedback Sent";
                    //_toEmail = _Request.PlanningAndDev_UserName + "@pscc.med.sa";
                    //_Email.SendTo(message, _toEmail, Subject);

                    myDB.Entry(_Request).State = EntityState.Modified;
                    _ = myDB.SaveChanges();

                    Success(string.Format("Feedback For Request <strong>({0})</strong> has been sent successfully.", Model.Request.ID));
                }
            }
            catch
            {
                Danger("Failed to send.");
            }

            return RedirectToAction("Manage", new { id = Model.Request.ID });
        }

        #endregion

        #region Json

        public JsonResult CheckAllowedRequest()
        {
            return Json(_Permission.CheckAllowedRequest(GetUserName()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckIfGenericUser()
        {
            return Json(CheckGenericUser(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
