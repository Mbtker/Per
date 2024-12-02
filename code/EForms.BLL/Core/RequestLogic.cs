using Performance.Management.BLL.Helper;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DAL.Repository;
using Performance.Management.DML.MainEntities;
using Performance.Management.DML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Performance.Management.BLL.Core
{
    public class RequestLogic
    {
        readonly RequestRepository _RequestRepository = new RequestRepository();

        public bool? IsSupervisor(string userId)
        {
            return _RequestRepository.IsSupervisorUser(userId);
        }

        public int SupervisorsCount()
        {
            return _RequestRepository.GetSupervisorsCount();
        }

        public bool IsHRAllowedUser(string UserName)
        {
            return _RequestRepository.IsHRAllowedUser(UserName);
        }

        public bool IsPerformanceUntiUser(string UserName)
        {
            return _RequestRepository.PerformanceUntiUser(UserName);
        }

        public bool CheckUserAccessToRequest(int ID, string UserName)
        {
            return _RequestRepository.CheckUserAccessToRequest(ID, UserName);
        }

        public IQueryable<Request> GetAllRequest()
        {
            return _RequestRepository.GetAllRequest();
        }

        public List<Request> GetAllRequest(string UserName)
        {
            return _RequestRepository.GetAllRequest(UserName).ToList();
        }

        public List<Request> GetAllRequest(JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = GetAllRequest();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower())));
            }

            TotalDisplayRecords = Requests.Count();

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID);
            else
                Requests = Requests.OrderByDescending(S => S.ID);


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength);

            return Requests.ToList();
        }

        public List<Request> GetAllRequest(string UserName, JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = GetAllRequest(UserName);

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public List<Request> GetAllUnDeletedRequest(JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetAllUnDeletedRequest().ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public List<Request> GetAllUnDeletedRequest(string UserName, JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetAllUnDeletedRequest(UserName).ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public List<RequestDatatable> GetAllRequests(string lang, string UserName, JQueryDataTableParams Params, int TypeID, out int TotalCount, out int TotalDisplayRecords, int Status = 0)
        {
            Entities db = new Entities();

            IQueryable<Request> requests = db.Requests;

            Expression<Func<Request, bool>> predicate2 = x => x.StatusID == Status;

            bool hrPerformanceUntiUser = db.HRSAllowedUsers.Any(x => x.UserName == UserName && x.PositionName == "PerformanceUnit" && x.IsActive == true);

            if (hrPerformanceUntiUser)
            {
                var pendingStatusList = db.Status.Where(x => (x.Name.Contains("Performance Unit") && x.IsPending == true) || x.Name.Contains("Rejected By Employee")).Select(x => x.ID).ToList();

                Expression<Func<Request, bool>> predicate = x => pendingStatusList.Contains(x.StatusID) && x.IsDeleted == false;

                if (Status > 0)
                {
                    requests = db.Requests.Where(predicate).Where(predicate2);
                }
                else
                {
                    requests = db.Requests.Where(predicate);
                }
            }
            else
            {
                Expression<Func<Request, bool>> predicate = x => x.IsDeleted == false &&
                            (x.UserName == UserName ||
                             x.PendingToUserName == UserName ||
                             x.DeptHead_UserName == UserName ||
                             x.DirectManager_UserName == UserName ||
                             x.Employee_UserName == UserName ||
                             x.HR_Director_UserName == UserName ||
                             x.HR_ManagerOfPersonal_UserName == UserName ||
                             x.HR_PerformanceUnit_UserName == UserName ||
                             x.RequesterUserName == UserName);

                requests = db.Requests.Where(predicate);

                if (Status > 0)
                {
                    requests = db.Requests.Where(predicate).Where(predicate2);
                }
                else
                {
                    requests = db.Requests.Where(predicate);
                }
            }

            TotalCount = requests.Count();

            if (Params.sSortDir_0 == "asc")
                requests = requests.OrderBy(S => S.ID);
            else
                requests = requests.OrderByDescending(S => S.ID);

            requests = requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength);

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                if (Params.sSearch.Length < 10 && int.TryParse(Params.sSearch, out int id))
                {
                    requests = requests.Where(S => S.ID == id);
                }
                else
                {
                    requests = requests.Where(S =>
                          ((!String.IsNullOrEmpty(S.RequesterUserName)) && S.RequesterUserName.ToLower().Contains(Params.sSearch.ToLower())) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower())));
                }
            }

            TotalDisplayRecords = requests.Count();

            List<RequestDatatable> requestDatatable = requests.ToList().Select(x => new RequestDatatable
            {
                ID = x.ID,
                TypeID = 1,
                RequesterUserEnglishName = x.RequesterUserEnglishName,
                EmployeeUserEnglishName = x.Employee_EnglishName,
                RequestDeptEnglishName = x.RequesterDeptEnglishName,
                RequestStatus = new RequestStatus
                {
                    StatusName = lang == "Ar" ? x.Status.NameAr : x.Status.Name,
                    StatusID = x.StatusID
                },
                AllowDelete = new AllowDelete
                {
                    RequestID = x.ID,
                    IsFinished = x.IsFinished,
                    IsRequestUserName = x.RequesterUserName != null && x.RequesterUserName.ToLower() == UserName.ToLower(),
                    CanDelete = (x.DeptHead_Approval == null && x.IsFinished == false),
                    CanAction = x.DeptHead_UserName == UserName ||
                                x.DirectManager_UserName == UserName ||
                                x.Employee_UserName == UserName ||
                                x.HR_Director_UserName == UserName ||
                                x.HR_ManagerOfPersonal_UserName == UserName ||
                                x.HR_PerformanceUnit_UserName == UserName ||
                                hrPerformanceUntiUser
                },
                DateTime = (x.CreateDateTime != null) ? x.CreateDateTime.Value.ToString("dd/MM/yyyy") : null,
                DeadlineDate = (x.DeadlineDate != null) ? x.DeadlineDate.Value.ToString("dd/MM/yyyy") : null,
                RequestType = lang == "Ar" ? x.RequestType.NameAr : x.RequestType.NameEn,
            }).ToList();

            return requestDatatable;
        }

        public List<RequestDatatable> GetAllRequests(string lang, string UserName, JQueryDataTableParams Params, string workEntity = "-1", int Status = -1)
        {
            Entities db = new Entities();

            int year = DateTime.Now.Year;

            IQueryable<Request> requests = db.Requests.Where(x => x.CreateDateTime.Value.Year == year && x.IsDeleted == false);

            if (Status != -1)
            {
                if (Status == (int)RequestStatusEnum.ApprovedByHRPerformanceUnit)
                {
                    requests = requests.Where(x => x.HR_PerformanceUnit_Approval == true);
                }
                else if (Status == (int)RequestEditableStatusEnum.RejectedByHRPerformanceUnit)
                {
                    requests = requests.Where(x => x.HR_PerformanceUnit_Approval == false);
                }
                else if (Status == (int)RequestEditableStatusEnum.RejectedByEmployeeFirstTime || Status == (int)RequestEditableStatusEnum.RejectedByEmployeeSecondTime)
                {
                    if (Status == (int)RequestEditableStatusEnum.RejectedByEmployeeFirstTime)
                    {
                        requests = requests.Where(x => x.IsRejectedByEmpFirstTime == true);
                    }
                    else
                    {
                        requests = requests.Where(x => x.IsRejectedByEmpSecondTime == true);
                    }
                }
                else
                {
                    Expression<Func<Request, bool>> status_predicate = x => x.StatusID == Status;
                    requests = requests.Where(status_predicate);
                }
            }

            if (workEntity != "-1")
            {
                Expression<Func<Request, bool>> workEntity_predicate = x => x.RequesterDeptCode == workEntity;
                requests = requests.Where(workEntity_predicate);
            }

            //TotalCount = requests.Count();

            if (Params.sSortDir_0 == "asc")
                requests = requests.OrderBy(S => S.ID);
            else
                requests = requests.OrderByDescending(S => S.ID);

            //requests = requests
            //         .Skip(Params.iDisplayStart)
            //         .Take(Params.iDisplayLength);

            //if (!string.IsNullOrEmpty(Params.sSearch))
            //{
            //    if (Params.sSearch.Length < 10 && int.TryParse(Params.sSearch, out int id))
            //    {
            //        requests = requests.Where(S => S.ID == id);
            //    }
            //    else
            //    {
            //        requests = requests.Where(S =>
            //              ((!String.IsNullOrEmpty(S.RequesterUserName)) && S.RequesterUserName.ToLower().Contains(Params.sSearch.ToLower())) ||
            //              ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
            //                (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower())));
            //    }
            //}

            //TotalDisplayRecords = requests.Count();

            List<RequestDatatable> requestDatatable = requests.ToList().Select(x => new RequestDatatable
            {
                ID = x.ID,
                TypeID = 1,
                RequesterUserEnglishName = x.RequesterUserEnglishName,
                EmployeeUserEnglishName = x.Employee_EnglishName,
                EmployeeUserName = x.Employee_UserName,
                StaffID = x.Employee_StaffID,
                RequestDeptEnglishName = x.RequesterDeptEnglishName,
                RequestStatus = new RequestStatus
                {
                    StatusName = lang == "Ar" ? x.Status.NameAr : x.Status.Name,
                    StatusID = x.StatusID
                },
                AllowDelete = new AllowDelete
                {
                    RequestID = x.ID,
                    IsFinished = x.IsFinished,
                    IsRequestUserName = x.RequesterUserName != null && x.RequesterUserName.ToLower() == UserName.ToLower(),
                    CanDelete = (x.DeptHead_Approval == null && x.IsFinished == false),
                    CanAction = x.DeptHead_UserName == UserName ||
                                x.DirectManager_UserName == UserName ||
                                x.Employee_UserName == UserName ||
                                x.HR_Director_UserName == UserName ||
                                x.HR_ManagerOfPersonal_UserName == UserName ||
                                x.HR_PerformanceUnit_UserName == UserName
                },
                DateTime = (x.CreateDateTime != null) ? x.CreateDateTime.Value.ToString("dd/MM/yyyy") : null,
                DeadlineDate = (x.DeadlineDate != null) ? x.DeadlineDate.Value.ToString("dd/MM/yyyy") : null,
                RequestType = lang == "Ar" ? x.RequestType.NameAr : x.RequestType.NameEn,
            }).ToList();

            return requestDatatable;
        }

        public List<Request> GetAllDeletedRequest(JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetAllDeletedRequest().ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public List<Request> GetAllDeletedRequest(string UserName, JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetAllDeletedRequest(UserName).ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public List<Request> GetRejetedRequest(JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetRejetedRequest().ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public Request GetRequest(int RequestID)
        {
            return _RequestRepository.GetRequest(RequestID);
        }

        public Request GetRequestReport(int RequestID)
        {
            return _RequestRepository.GetRequest(RequestID);
        }

        public Request GetEmployeeWorkEntityByIDentityNo(string UserName)
        {
            return _RequestRepository.GetEmployeeWorkEntityByIDentityNo(UserName);
        }

        #region Dashboard

        public int GetAllRequestCount()
        {
            return _RequestRepository.GetAllRequestCount();
        }

        public int GetFinishedRequestCount()
        {
            return _RequestRepository.GetFinishedRequestCount();
        }

        public int GetUnderProcessRequestCount()
        {
            return _RequestRepository.GetUnderProcessRequestCount();
        }

        public int GetDelayRequestCount()
        {
            return _RequestRepository.GetDelayRequestCount();
        }

        public int GetDepartmentApprovalCount()
        {
            return _RequestRepository.GetDepartmentApprovalCount();
        }

        public double GetDepartmentApprovalRatio()
        {
            return _RequestRepository.GetDepartmentApprovalRatio();
        }

        public int GetDepartmentDispprovalCount()
        {
            return _RequestRepository.GetDepartmentDisapprovalCount();
        }

        public double GetDepartmentDispprovalRatio()
        {
            return _RequestRepository.GetDepartmentDisapprovalRatio();
        }

        public int GetHRApprovalCount()
        {
            return _RequestRepository.GetHRApprovalCount();
        }

        public double GetHRApprovalRatio()
        {
            return _RequestRepository.GetHRApprovalRatio();
        }

        public int GetRequestRejectedCount()
        {
            return _RequestRepository.GetRequestRejectedCount();
        }

        public double GetRequestDisapprovalRatio()
        {
            return _RequestRepository.GetRequestDisapprovalRatio();
        }

        public int GetAchievementFeedback25Count()
        {
            return _RequestRepository.GetAchievementFeedback25Count();
        }
        public int GetAchievementFeedback50Count()
        {
            return _RequestRepository.GetAchievementFeedback50Count();
        }
        public int GetAchievementFeedback75Count()
        {
            return _RequestRepository.GetAchievementFeedback75Count();
        }
        public int GetAchievementFeedback100Count()
        {
            return _RequestRepository.GetAchievementFeedback100Count();
        }
        public int GetTimeOfServiceFeedback25Count()
        {
            return _RequestRepository.GetTimeOfServiceFeedback25Count();
        }
        public int GetTimeOfServiceFeedback50Count()
        {
            return _RequestRepository.GetTimeOfServiceFeedback50Count();
        }
        public int GetTimeOfServiceFeedback75Count()
        {
            return _RequestRepository.GetTimeOfServiceFeedback75Count();
        }
        public int GetTimeOfServiceFeedback100Count()
        {
            return _RequestRepository.GetTimeOfServiceFeedback100Count();
        }
        public int GetSatisfacationFeedback25Count()
        {
            return _RequestRepository.GetSatisfacationFeedback25Count();
        }
        public int GetSatisfacationFeedback50Count()
        {
            return _RequestRepository.GetSatisfacationFeedback50Count();
        }
        public int GetSatisfacationFeedback75Count()
        {
            return _RequestRepository.GetSatisfacationFeedback75Count();
        }
        public int GetSatisfacationFeedback100Count()
        {
            return _RequestRepository.GetSatisfacationFeedback100Count();
        }

        public int GetFormAdminFinishedCount()
        {
            return _RequestRepository.GetFormAdminFinishedCount();
        }

        public double GetFormAdminFinishedRatio()
        {
            return _RequestRepository.GetFormAdminFinishedRatio();
        }

        public double GetFormAdminUnderProcessingRatio()
        {
            return _RequestRepository.GetFormAdminUnderProcessingRatio();
        }

        public double GetFormAdminDelaydRatio()
        {
            return _RequestRepository.GetFormAdminDelaydRatio();
        }

        #endregion

        public int InsertRequest(Request _Request)
        {
            return _RequestRepository.InsertRequest(_Request);
        }

        public int UpdateRequest(Request _Request)
        {
            return _RequestRepository.UpdateRequest(_Request);
        }

        public int DeleteRequest(Request _Request)
        {
            return _RequestRepository.DeleteRequest(_Request);
        }

        public int DeleteRequest(int RequestID)
        {
            return _RequestRepository.DeleteRequest(RequestID);
        }
        public List<Request> GetAllResolvedRequest(string UserName, JQueryDataTableParams Params, out int TotalCount, out int TotalDisplayRecords)
        {
            var Requests = _RequestRepository.GetAllResolvedRequest(UserName).ToList();

            TotalCount = Requests.Count();

            if (!string.IsNullOrEmpty(Params.sSearch))
            {
                Requests = Requests.Where(S =>
                           S.ID.ToString().Contains(Params.sSearch) ||
                           ((S.CreateDateTime != null) && S.CreateDateTime.Value.ToString().Contains(Params.sSearch)) ||
                          ((!String.IsNullOrEmpty(S.RequesterDeptEnglishName)) && S.RequesterDeptEnglishName.ToLower().Contains(Params.sSearch.ToLower())) ||
                            (S.Status != null && S.Status.Name.ToLower().Contains(Params.sSearch.ToLower()))).ToList();
            }

            TotalDisplayRecords = Requests.Count;

            //Func<Request, string> orderingFunction = (m => Params.iSortCol_0 == 0 ? m.ID.ToString() :
            //                     (Params.iSortCol_0 == 1 && !String.IsNullOrEmpty(m.Name)) ? m.Name :
            //                     Params.iSortCol_0 == 2 ? m.DayNo :m.StartDate.Value.ToString());

            if (Params.sSortDir_0 == "asc")
                Requests = Requests.OrderBy(S => S.ID).ToList();
            else
                Requests = Requests.OrderByDescending(S => S.ID).ToList();


            Requests = Requests
                     .Skip(Params.iDisplayStart)
                     .Take(Params.iDisplayLength).ToList();

            return Requests;
        }

        public int GetEmployeesCount()
        {
            throw new NotImplementedException();
        }

        public int GetEmployeesSupervisoryCount()
        {
            throw new NotImplementedException();
        }

        public int GetEmployeesNonSupervisoryCount()
        {
            throw new NotImplementedException();
        }

        public int GetEmployeesNonSupervisoryCompetenciesOnlyCount()
        {
            throw new NotImplementedException();
        }

        public int GetEmployeesUnknownCount()
        {
            throw new NotImplementedException();
        }
    }
}
