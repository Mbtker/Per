using Performance.Management.BLL.Core;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.MainEntities;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Performance.Management.BLL.Helper
{
    public class Permission
    {
        readonly RequestLogic _RequestLogic = new RequestLogic();
        readonly OasisSync _OasisSync = new OasisSync();

        readonly HRService.HRClient _HRClient = new HRService.HRClient();
        readonly QryExe.QryExeClient QryExe = new QryExe.QryExeClient();

        public bool CheckCurrentUserIsManager(string userName)
        {
           var empList = _OasisSync.GetMyReportingEmployeesByIDentityNo(userName);

            return empList?.Count > 0;
        }

        public bool CheckAllowedRequest(string UserName)
        {
            Employee _Employee = _OasisSync.GetEmployeeByUserName(UserName);

            if (_Employee != null)
            {
                //AllowedUserType _AllowedUserType = _AllowedUserTypeLogic.GetAllAllowedUserType().Where(A => A.Type == _Employee.POSITION_TYPE).SingleOrDefault();

                //if (_AllowedUserType != null)
                //{
                //    return true;
                //}
            }

            return false;
        }

        public bool CheckUserInDeptHeadRequests(string UserName)
        {
            return _RequestLogic.GetAllRequest().Any(R => R.DeptHead_UserName != null && R.DeptHead_UserName.ToLower() == UserName.ToLower());
        }

        public bool CheckUserInDeptHeadSingleRequest(string UserName, int RequestID)
        {
            return _RequestLogic.GetAllRequest().Any(R => R.ID == RequestID && R.DeptHead_UserName != null && R.DeptHead_UserName.ToLower() == UserName.ToLower());
        }

        public bool CheckUserIsHR(string userName)
        {
            var HREmployess = _OasisSync.GetAllEmployeeByDeptCode(ConfigurationManager.AppSettings["Dept.HR"]);

            return HREmployess != null && HREmployess.FirstOrDefault(x => x.DOC_NUMBER == userName) != null;
        }

        public bool CheckUserIsRequestUserSingleRequest(string UserName, int RequestID)
        {
            return _RequestLogic.GetAllRequest().Any(R => R.ID == RequestID && (R.UserName == UserName ||
                                                     //x.DeleteUserName == UserName ||
                                                     R.DeptHead_UserName == UserName ||
                                                     R.DirectManager_UserName == UserName ||
                                                     R.Employee_UserName == UserName ||
                                                     R.HR_Director_UserName == UserName ||
                                                     R.HR_ManagerOfPersonal_UserName == UserName ||
                                                     R.HR_PerformanceUnit_UserName == UserName ||
                                                     R.RequesterUserName == UserName));
        }
    }
}
