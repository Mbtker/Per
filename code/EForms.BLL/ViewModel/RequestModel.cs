using Performance.Management.BLL.ViewModel.Enums;
using Performance.Management.DML.MainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Performance.Management.BLL.ViewModel
{
    public class RequestModel
    {
        public RequestModel()
        {
        }

        public Request Request { get; set; }
        public List<Employee> MyEmployees { get; set; }
        public Employee SelectedEmployee { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Department> Department { get; set; }
        public List<SAction> SActionList { get; set; }
        public List<Reason> ReasonList { get; set; }
        public bool IsFinished { get; set; }
        public bool FeedBackGiven { get; set; }
        public string CurrentUserName { get; set; }
        public string Language { get; set; }
        public RequestTypeEnum Grade { get; set; }
        public bool IsHRUser { get; set; }
        public RequestManager Manager { get; set; }
        public List<GoalViewModel> Goals { get; set; }
        public List<CompetencyViewModel> Competencies { get; set; }
        public List<RequestType> RequestTypes { get; set; }
        public bool IsPerformanceUnitUser { get; set; }
        public string DeptHead_UserName { get; set; }
        public string Employee_UserName { get; set; }
        public string DirectManager_UserName { get; set; }
        public string HR_ManagerOfPersonal_UserName { get; set; }
        public string HR_Director_UserName { get; set; }
        public bool FormSubmit { get; set; }
        public int Year { get; set; }
        public int RequestTypeID { get; set; }
        public bool IsRequestExist { get; set; }
        public string EditMode { get; set; }
    }

    public class RequestManager
    {
        public ManageStatusEnum ManageStatus { get; set; }
        public string UserName { get; set; }
        public bool IsCurrentUserHasAccess { get; set; }
    }
}
