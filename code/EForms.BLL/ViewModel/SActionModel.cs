using Performance.Management.DML.MainEntities;
using System.Collections.Generic;

namespace Performance.Management.BLL.ViewModel
{
    public class SActionModel
    {

        public SAction SAction { get; set; }
        public List<SAction> SActionList { get; set; }
        public List<Employee> EmployeeList { get; set; }
    }
}
