//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Performance.Management.DML.PSCCSetting
{
    using System;
    using System.Collections.Generic;
    
    public partial class DepartmentHead
    {
        public int ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentHeadStaffID { get; set; }
        public string DepartmentHeadUserName { get; set; }
        public string DepartmentHeadFullName { get; set; }
        public string DepartmentHeadPositionCode { get; set; }
        public string DepartmentHeadPositionName { get; set; }
        public string DepartmentDeputyDirectorStaffID { get; set; }
        public string DepartmentDeputyDirectorUserName { get; set; }
        public string DepartmentDeputyDirectorFullName { get; set; }
        public string DepartmentDeputyDirectorPositionCode { get; set; }
        public string DepartmentDeputyDirectorPositionName { get; set; }
        public string DepartmentActingDirectorStaffID { get; set; }
        public string DepartmentActingDirectorUserName { get; set; }
        public string DepartmentActingDirectorFullName { get; set; }
        public string DepartmentActingDirectorPositionCode { get; set; }
        public string DepartmentActingDirectorPositionName { get; set; }
        public Nullable<int> DirectorTypeID { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual DirectorType DirectorType { get; set; }
    }
}
