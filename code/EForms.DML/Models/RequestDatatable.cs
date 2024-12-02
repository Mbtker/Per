using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Management.DML.Models
{
    public class RequestDatatable
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public string RequestDeptEnglishName { get; set; }
        public string DateTime { get; set; }
        public string DeadlineDate { get; set; }
        public string RequestType { get; set; }
        public AllowDelete AllowDelete { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string RequesterUserEnglishName { get; set; }
        public string EmployeeUserEnglishName { get; set; }
        public string EmployeeUserName { get; set; }
        public string StaffID { get; set; }
    }
}
