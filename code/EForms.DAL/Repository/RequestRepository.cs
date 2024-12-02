using Performance.Management.DML.MainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Performance.Management.DAL.Repository
{
    public class RequestRepository
    {
        readonly Entities db = new Entities();

        public bool? IsSupervisorUser(string userId)
        {
            return db.SupervisorUsers.Any(x => x.UserId == userId);
        }

        public int GetSupervisorsCount()
        {
            return db.SupervisorUsers.Count(x => x.IsSupervisor == true);
        }

        public IQueryable<Request> GetAllRequest()
        {
            return db.Requests;
        }

        public bool IsHRAllowedUser(string UserName)
        {
            return db.HRSAllowedUsers.Any(x => x.UserName == UserName);
        }

        public bool PerformanceUntiUser(string UserName)
        {
            return db.HRSAllowedUsers.Any(x => x.UserName == UserName && x.PositionName == "PerformanceUnit" && x.IsActive == true);
        }

        public IQueryable<HRSAllowedUser> GetHRAllowedUsers()
        {
            return db.HRSAllowedUsers;
        }

        public bool CheckUserAccessToRequest(int ID, string UserName)
        {
            return db.Requests.Any(x => x.ID == ID && (x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsDeleted == false
                        );
        }

        public IQueryable<Request> GetAllRequest(string UserName)
        {
            return db.Requests.Where(x => (x.UserName == UserName ||
                         //x.DeleteUserName == UserName ||
                         x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsDeleted == false
                        ).OrderByDescending(R => R.ID);
        }

        public IQueryable<Request> GetAllUnDeletedRequest()
        {
            return db.Requests.Where(R => R.IsDeleted == false).OrderByDescending(R => R.ID);
        }

        public IQueryable<Request> GetAllUnDeletedRequest(string UserName)
        {
            return db.Requests.Where(x => (x.UserName == UserName ||
                         //x.DeleteUserName == UserName ||
                         x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsDeleted == false
                        ).OrderByDescending(R => R.ID);
        }

        public IQueryable<Request> GetAllPendingRequest(string UserName, int TypeID)
        {
            IQueryable<Request> requests;

            var pendingStatus = db.Status.Where(x => x.IsPending == true);

            requests = db.Requests.Where(x => pendingStatus.Contains(x.Status) &&
                        (x.UserName == UserName ||
                         //x.DeleteUserName == UserName ||
                         x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsFinished == false && x.IsDeleted == false
                        ).OrderByDescending(R => R.ID);

            return requests;
        }

        public IQueryable<Request> GetAllDeletedRequest()
        {
            return db.Requests.Where(R => R.IsDeleted != true); ;
        }

        public IQueryable<Request> GetAllDeletedRequest(string UserName)
        {
            return db.Requests.Where(x => (x.UserName == UserName ||
                         //x.DeleteUserName == UserName ||
                         x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsDeleted == true
                        ).OrderByDescending(R => R.ID);
        }

        public IQueryable<Request> GetRejetedRequest()
        {
            var rejectedStatus = db.Status.Where(x => x.IsReject == true).Select(x => x.ID);

            return db.Requests.Where(R => rejectedStatus.Contains(R.StatusID) && R.IsDeleted == false).OrderByDescending(R => R.ID);
        }

        public Request GetRequest(object RequestID)
        {
            Request _Request = db.Requests.Find(RequestID);
            if (_Request != null)
            {
                return _Request;
            }
            return null;
        }

        public Request GetEmployeeWorkEntityByIDentityNo(string UserName)
        {
            return db.Requests.Where(R => R.RequesterUserName == UserName).OrderByDescending(R => R.ID).FirstOrDefault();
        }

        #region Dashboard

        public int GetAllRequestCount()
        {
            return db.Requests.Count();
        }

        public int GetFinishedRequestCount()
        {
            return db.Requests.Where(R => R.IsFinished == true).Count();
        }

        public int GetUnderProcessRequestCount()
        {
            return db.Requests.Where(R => R.IsFinished == false && R.IsDeleted == false).Count();
        }

        public int GetDelayRequestCount()
        {
            return db.Requests.Where(R => R.IsFinished == false && R.DeadlineDate.Value < DateTime.Now).Count();
        }

        public int GetDepartmentApprovalCount()
        {
            return db.Requests.Where(R => R.DeptHead_Approval != null && R.DeptHead_Approval == true).Count();
        }

        public double GetDepartmentApprovalRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Approvals = GetDepartmentApprovalCount();
            float Ratio = Approvals / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public int GetDepartmentDisapprovalCount()
        {
            return db.Requests.Where(R => R.DeptHead_Approval != null && R.DeptHead_Approval == false).Count();
        }

        public double GetDepartmentDisapprovalRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Dispprovals = GetDepartmentDisapprovalCount();
            float Ratio = Dispprovals / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public int GetHRApprovalCount()
        {
            return db.Requests.Where(R => R.IsFinished == true).Count();
        }

        public double GetHRApprovalRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Approvals = GetHRApprovalCount();
            float Ratio = Approvals / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public int GetRequestRejectedCount()
        {
            var rejectedStatus = db.Status.Where(x => x.IsReject == true).Select(x => x.ID);

            return db.Requests.Where(R => rejectedStatus.Contains(R.StatusID) && R.IsDeleted == false).Count();
        }

        public double GetRequestDisapprovalRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Dispprovals = GetRequestRejectedCount();
            float Ratio = Dispprovals / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public int GetFormAdminFinishedCount()
        {
            return db.Requests.Where(R => R.HR_PerformanceUnit_Approval != null && R.HR_PerformanceUnit_Approval == true).Count();
        }

        public double GetFormAdminFinishedRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Finished = GetFormAdminFinishedCount();
            float Ratio = Finished / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public double GetFormAdminUnderProcessingRatio()
        {
            float AllRequests = GetAllRequestCount();
            float UnderProcessing = GetUnderProcessRequestCount();
            float Ratio = UnderProcessing / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public double GetFormAdminDelaydRatio()
        {
            float AllRequests = GetAllRequestCount();
            float Delayed = GetDelayRequestCount();
            float Ratio = Delayed / AllRequests * 100;
            return Math.Round(Convert.ToDouble(Ratio), 1);
        }

        public int GetAchievementFeedback25Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.AchievementFeedBack == 25).Count();
        }
        public int GetAchievementFeedback50Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.AchievementFeedBack == 50).Count();
        }
        public int GetAchievementFeedback75Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.AchievementFeedBack == 75).Count();
        }
        public int GetAchievementFeedback100Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.AchievementFeedBack == 100).Count();
        }
        public int GetTimeOfServiceFeedback25Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.TimeFeedBack == 25).Count();
        }
        public int GetTimeOfServiceFeedback50Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.TimeFeedBack == 50).Count();
        }
        public int GetTimeOfServiceFeedback75Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.TimeFeedBack == 75).Count();
        }
        public int GetTimeOfServiceFeedback100Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.TimeFeedBack == 100).Count();
        }
        public int GetSatisfacationFeedback25Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.SatisfacationFeedBack == 25).Count();
        }
        public int GetSatisfacationFeedback50Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.SatisfacationFeedBack == 50).Count();
        }
        public int GetSatisfacationFeedback75Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.SatisfacationFeedBack == 75).Count();
        }
        public int GetSatisfacationFeedback100Count()
        {
            return db.Requests.Where(R => R.FeedBackGiven != null && R.FeedBackGiven == true && R.SatisfacationFeedBack == 100).Count();
        }

        #endregion

        public int InsertRequest(Request _Request)
        {
            if (_Request != null)
            {
                _ = db.Requests.Add(_Request);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateRequest(Request _Request)
        {
            if (_Request != null)
            {
                db.Entry(_Request).State = EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteRequest(Request _Request)
        {
            if (_Request != null)
            {
                _ = db.Requests.Remove(_Request);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteRequest(int RequestID)
        {
            Request _Request = db.Requests.Find(RequestID);
            if (_Request != null)
            {
                _ = db.Requests.Remove(_Request);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<Request> GetAllResolvedRequest(string UserName)
        {
            return db.Requests.Where(x => (x.UserName == UserName ||
                         //x.DeleteUserName == UserName ||
                         x.DeptHead_UserName == UserName ||
                         x.DirectManager_UserName == UserName ||
                         x.Employee_UserName == UserName ||
                         x.HR_Director_UserName == UserName ||
                         x.HR_ManagerOfPersonal_UserName == UserName ||
                         x.HR_PerformanceUnit_UserName == UserName ||
                         x.RequesterUserName == UserName)
                        && x.IsDeleted == true && x.IsFinished == true
                        ).OrderByDescending(R => R.ID);
        }

        public Request GetRequest(int requestID)
        {
            return db.Requests
                .Include(x => x.RequestType)
                .Include(x => x.Goals)
                .Include(x => x.Goals.Select(s => s.CompetencyLookup))
                .Include(x => x.Goals.Select(s => s.CompetencyLookup.CompetencyDetailsLookups))
                .FirstOrDefault(x => x.ID == requestID);
        }
    }
}
