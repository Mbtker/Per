using Performance.Management.BLL.Core;
using Performance.Management.BLL.Helper;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.MainEntities;
using Performance.Management.UIL.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Web.Mvc;

namespace Performance.Management.UIL.Controllers
{
    public class HomeController : BaseController
    {
        #region Helper
        public class InMemoryCache : ICacheService
        {
            public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
            {
                if (!(MemoryCache.Default.Get(cacheKey) is T item))
                {
                    item = getItemCallback();

                    if (item != null)
                    {
                        _ = MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddDays(1));
                    }
                }

                return item;
            }
        }

        interface ICacheService
        {
            T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
        }

        #endregion

        readonly InMemoryCache memoryCache = new InMemoryCache();

        readonly Employee _Employee = new Employee();
        readonly OasisSync _OasisSync = new OasisSync();

        readonly RequestLogic _RequestLogic = new RequestLogic();
        HomeModel _HomeModel = new HomeModel();
        readonly Permission _Permission = new Permission();
        readonly StatusLogic _StatusLogic = new StatusLogic();

        public ActionResult HRDashboard()
        {
            if (!_Permission.CheckUserIsHR(GetUserName()))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            _HomeModel.AllRequestCount = _RequestLogic.GetAllRequestCount();
            _HomeModel.FinishedRequestCount = _RequestLogic.GetFinishedRequestCount();
            _HomeModel.UnderProcessRequestCount = _RequestLogic.GetUnderProcessRequestCount();
            _HomeModel.DelayRequestCount = _RequestLogic.GetDelayRequestCount();

            _HomeModel.DepartmentApprovalCount = _RequestLogic.GetDepartmentApprovalCount();
            _HomeModel.DepartmentApprovalRatio = _RequestLogic.GetDepartmentApprovalRatio();
            _HomeModel.DepartmentDisapprovalCount = _RequestLogic.GetDepartmentDispprovalCount();
            _HomeModel.DepartmentDisapprovalRatio = _RequestLogic.GetDepartmentDispprovalRatio();

            _HomeModel.PORApprovalCount = _RequestLogic.GetHRApprovalCount();
            _HomeModel.PORApprovalRatio = _RequestLogic.GetHRApprovalRatio();
            _HomeModel.PRDisapprovalCount = _RequestLogic.GetRequestRejectedCount();
            _HomeModel.PRDisapprovalRatio = _RequestLogic.GetRequestDisapprovalRatio();

            _HomeModel.AchievementFeedback25Count = _RequestLogic.GetAchievementFeedback25Count();
            _HomeModel.AchievementFeedback50Count = _RequestLogic.GetAchievementFeedback50Count();
            _HomeModel.AchievementFeedback75Count = _RequestLogic.GetAchievementFeedback75Count();
            _HomeModel.AchievementFeedback100Count = _RequestLogic.GetAchievementFeedback100Count();

            _HomeModel.TimeOfServiceFeedback25Count = _RequestLogic.GetTimeOfServiceFeedback25Count();
            _HomeModel.TimeOfServiceFeedback50Count = _RequestLogic.GetTimeOfServiceFeedback50Count();
            _HomeModel.TimeOfServiceFeedback75Count = _RequestLogic.GetTimeOfServiceFeedback75Count();
            _HomeModel.TimeOfServiceFeedback100Count = _RequestLogic.GetTimeOfServiceFeedback100Count();

            _HomeModel.SatisfacationFeedback25Count = _RequestLogic.GetSatisfacationFeedback25Count();
            _HomeModel.SatisfacationFeedback50Count = _RequestLogic.GetSatisfacationFeedback50Count();
            _HomeModel.SatisfacationFeedback75Count = _RequestLogic.GetSatisfacationFeedback75Count();
            _HomeModel.SatisfacationFeedback100Count = _RequestLogic.GetSatisfacationFeedback100Count();

            _HomeModel.FormAdminFinishedCount = _RequestLogic.GetFormAdminFinishedCount();
            _HomeModel.FormAdminFinishedRatio = _RequestLogic.GetFormAdminFinishedRatio();
            _HomeModel.FormAdminUnderProcessingCount = _RequestLogic.GetUnderProcessRequestCount();
            _HomeModel.FormAdminUnderProcessingRatio = _RequestLogic.GetFormAdminUnderProcessingRatio();
            _HomeModel.FormAdminDelaydCount = _RequestLogic.GetDelayRequestCount();
            _HomeModel.FormAdminDelaydRatio = _RequestLogic.GetFormAdminDelaydRatio();

            //_HomeModel.EmpCount = _RequestLogic.GetEmployeesCount();
            //_HomeModel.EmpSupervisoryCount = _RequestLogic.GetEmployeesSupervisoryCount();
            //_HomeModel.EmpNonSupervisoryCount = _RequestLogic.GetEmployeesNonSupervisoryCount();
            //_HomeModel.EmpNonSupervisoryCompetenciesOnlyCount = _RequestLogic.GetEmployeesNonSupervisoryCompetenciesOnlyCount();
            //_HomeModel.EmpAllNonSupervisoryCount = _HomeModel.EmpNonSupervisoryCount + _HomeModel.EmpNonSupervisoryCompetenciesOnlyCount;
            //_HomeModel.EmpNonUnknownCount = _RequestLogic.GetEmployeesUnknownCount();

            var empList = base.GetEmpList().Where(x => x.CONT_END_DATE >= DateTime.Now.Date && (x.LAST_WORKING_DATE == null || x.LAST_WORKING_DATE >= DateTime.Now.Date));

            _HomeModel.EmpCount = empList.Count(d => d.CONTRACT_TYPE_DESC.Equals("INTERNATIONAL", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_DESC.Equals("LOCAL (40 DAYS)", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_DESC.Equals("LOCAL (30 DAYS)", StringComparison.OrdinalIgnoreCase));
            _HomeModel.EmpCountRatio = 100;

            //_HomeModel.EmpSupervisoryCount = empList.Count(x =>
            //{
            //    if (!string.IsNullOrWhiteSpace(x.GRADE_STEP))
            //    {
            //        string gradeString = x.GRADE_STEP.Substring(0, 2);

            //        if (int.TryParse(gradeString, out int grade))
            //        {
            //            return grade >= 13;
            //        }
            //    }

            //    return false;
            //});

            _HomeModel.EmpSupervisoryCount = _RequestLogic.SupervisorsCount();
            _HomeModel.EmpSupervisoryCountRatio = GetRation(_HomeModel.EmpSupervisoryCount, _HomeModel.EmpCount);

            //_HomeModel.EmpNonSupervisoryCount = empList.Count(x =>
            //{
            //    if (!string.IsNullOrWhiteSpace(x.GRADE_STEP))
            //    {
            //        string gradeString = x.GRADE_STEP.Substring(0, 2);

            //        if (int.TryParse(gradeString, out int grade))
            //        {
            //            return grade >= 6 && grade < 13;
            //        }
            //    }

            //    return false;
            //});

            _HomeModel.EmpNonSupervisoryCount = _HomeModel.EmpCount - _HomeModel.EmpSupervisoryCount;
            _HomeModel.EmpNonSupervisoryCountRatio = GetRation(_HomeModel.EmpNonSupervisoryCount, _HomeModel.EmpCount);

            //_HomeModel.EmpNonSupervisoryCompetenciesOnlyCount = _RequestLogic.GetEmployeesNonSupervisoryCompetenciesOnlyCount();
            //_HomeModel.EmpAllNonSupervisoryCount = _HomeModel.EmpNonSupervisoryCount + _HomeModel.EmpNonSupervisoryCompetenciesOnlyCount;
            //_HomeModel.EmpNonUnknownCount = 421;

            _HomeModel.JobCountTotal = 2675;
            _HomeModel.JobCountTotalRaion = 100;

            _HomeModel.JobCountForBusy = 2282;
            _HomeModel.JobCountForBusyRatio = 85;

            _HomeModel.JobCountForVacancy = 293;
            _HomeModel.JobCountForVacancyRatio = 11;

            return View(_HomeModel);
        }

        private double GetRation(float first, float second)
        {
            var ratio = (first / second) * 100;

            return Math.Round(ratio, 1);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Charts()
        {
            if (_Permission.CheckUserIsHR(GetUserName()))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var homeModel = memoryCache.GetOrSet($"HomeModel", () => GetHomeModel());

                stopwatch.Stop();
                homeModel.Elapsed = stopwatch.Elapsed.ToString();

                _HomeModel = homeModel;

                return View(homeModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        private HomeModel GetHomeModel()
        {
            var homeModel = new HomeModel();

            homeModel.AllRequestCount = _RequestLogic.GetAllRequestCount();
            homeModel.FinishedRequestCount = _RequestLogic.GetFinishedRequestCount();
            homeModel.UnderProcessRequestCount = _RequestLogic.GetUnderProcessRequestCount();
            homeModel.DelayRequestCount = _RequestLogic.GetDelayRequestCount();

            homeModel.DepartmentApprovalCount = _RequestLogic.GetDepartmentApprovalCount();
            homeModel.DepartmentDisapprovalCount = _RequestLogic.GetDepartmentDispprovalCount();

            homeModel.PORApprovalCount = _RequestLogic.GetHRApprovalCount();
            homeModel.PRDisapprovalCount = _RequestLogic.GetRequestRejectedCount();

            homeModel.FormAdminFinishedCount = _RequestLogic.GetFormAdminFinishedCount();
            homeModel.FormAdminDelaydCount = _RequestLogic.GetDelayRequestCount();

            return homeModel;
        }

        public ActionResult Stats()
        {
            if (_Permission.CheckUserIsHR(GetUserName()))
            {
                _HomeModel.AllRequestCount = _RequestLogic.GetAllRequestCount();
                _HomeModel.FinishedRequestCount = _RequestLogic.GetFinishedRequestCount();
                _HomeModel.UnderProcessRequestCount = _RequestLogic.GetUnderProcessRequestCount();
                _HomeModel.DelayRequestCount = _RequestLogic.GetDelayRequestCount();

                _HomeModel.DepartmentApprovalCount = _RequestLogic.GetDepartmentApprovalCount();
                _HomeModel.DepartmentApprovalRatio = _RequestLogic.GetDepartmentApprovalRatio();
                _HomeModel.DepartmentDisapprovalCount = _RequestLogic.GetDepartmentDispprovalCount();
                _HomeModel.DepartmentDisapprovalRatio = _RequestLogic.GetDepartmentDispprovalRatio();

                _HomeModel.PORApprovalCount = _RequestLogic.GetHRApprovalCount();
                _HomeModel.PORApprovalRatio = _RequestLogic.GetHRApprovalRatio();
                _HomeModel.PRDisapprovalCount = _RequestLogic.GetRequestRejectedCount();
                _HomeModel.PRDisapprovalRatio = _RequestLogic.GetRequestDisapprovalRatio();

                _HomeModel.AchievementFeedback25Count = _RequestLogic.GetAchievementFeedback25Count();
                _HomeModel.AchievementFeedback50Count = _RequestLogic.GetAchievementFeedback50Count();
                _HomeModel.AchievementFeedback75Count = _RequestLogic.GetAchievementFeedback75Count();
                _HomeModel.AchievementFeedback100Count = _RequestLogic.GetAchievementFeedback100Count();

                _HomeModel.TimeOfServiceFeedback25Count = _RequestLogic.GetTimeOfServiceFeedback25Count();
                _HomeModel.TimeOfServiceFeedback50Count = _RequestLogic.GetTimeOfServiceFeedback50Count();
                _HomeModel.TimeOfServiceFeedback75Count = _RequestLogic.GetTimeOfServiceFeedback75Count();
                _HomeModel.TimeOfServiceFeedback100Count = _RequestLogic.GetTimeOfServiceFeedback100Count();

                _HomeModel.SatisfacationFeedback25Count = _RequestLogic.GetSatisfacationFeedback25Count();
                _HomeModel.SatisfacationFeedback50Count = _RequestLogic.GetSatisfacationFeedback50Count();
                _HomeModel.SatisfacationFeedback75Count = _RequestLogic.GetSatisfacationFeedback75Count();
                _HomeModel.SatisfacationFeedback100Count = _RequestLogic.GetSatisfacationFeedback100Count();

                _HomeModel.FormAdminFinishedCount = _RequestLogic.GetFormAdminFinishedCount();
                _HomeModel.FormAdminFinishedRatio = _RequestLogic.GetFormAdminFinishedRatio();
                _HomeModel.FormAdminUnderProcessingCount = _RequestLogic.GetUnderProcessRequestCount();
                _HomeModel.FormAdminUnderProcessingRatio = _RequestLogic.GetFormAdminUnderProcessingRatio();
                _HomeModel.FormAdminDelaydCount = _RequestLogic.GetDelayRequestCount();
                _HomeModel.FormAdminDelaydRatio = _RequestLogic.GetFormAdminDelaydRatio();

                return View(_HomeModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        public ActionResult UserGuide()
        {
            return View();
        }

        public ActionResult MailBox()
        {
            return View();
        }

        public ActionResult Compose()
        {
            return View();
        }

        public ActionResult Read()
        {
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            //return RedirectToAction("Index2", "Home");

            return Redirect(GetPreviousUrl());
        }

        public JsonResult GetRequestCounter()
        {
            var UserName = GetUserName();
            int f = (int)RequestStatusEnum.Finished;

            Expression<Func<Request, bool>> predicate = x => x.UserName == UserName ||
                                                        x.DeptHead_UserName == UserName ||
                                                        x.DirectManager_UserName == UserName ||
                                                        x.Employee_UserName == UserName ||
                                                        x.HR_Director_UserName == UserName ||
                                                        x.HR_ManagerOfPersonal_UserName == UserName ||
                                                        x.HR_PerformanceUnit_UserName == UserName ||
                                                        x.PendingToUserName == UserName ||
                                                        x.RequesterUserName == UserName;

            var pendingStatusList = _StatusLogic.GetAll().Where(x => x.IsPending == true).Select(x => x.ID).ToList();

            var rejectedStatusList = _StatusLogic.GetAll().Where(x => x.IsReject == true).Select(x => x.ID).ToList();

            var pending = _RequestLogic.GetAllRequest().Where(predicate).Where(x => pendingStatusList.Contains(x.StatusID)).Count();

            if (_RequestLogic.IsPerformanceUntiUser(UserName))
            {
                pendingStatusList = _StatusLogic.GetAll().Where(x => (x.Name.Contains("Performance Unit") && x.IsPending == true) || x.Name.Contains("Rejected By Employee")).Select(x => x.ID).ToList();

                pending = _RequestLogic.GetAllRequest().Where(x => pendingStatusList.Contains(x.StatusID) && x.IsDeleted == false).Count();
            }

            var finished = _RequestLogic.GetAllRequest().Where(predicate).Where(x => x.StatusID == f).Count();
            var rejected = _RequestLogic.GetAllRequest().Where(predicate).Where(x => rejectedStatusList.Contains(x.StatusID)).Count();

            return Json(new { rejected, pending, finished }, JsonRequestBehavior.AllowGet);
        }
    }
}