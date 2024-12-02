using Performance.Management.DML.MainEntities;
using System.Collections.Generic;

namespace Performance.Management.BLL.ViewModel
{
    public class EmpDataModel
    {
        public EmpSearch Header { get; set; }
        public List<Department> Department { get; set; }
    }

    public class EmpDetails
    {
        public string Id { get; set; }
        public OasisUser Employee { get; set; }
        public bool IsSupervisor { get; set; }
        public int SupervisorUserId { get; set; }
        public List<Request> Requests { get; set; }
        public string UserId { get; set; }
    }
}
