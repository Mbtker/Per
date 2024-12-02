//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Performance.Management.DML.MainEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reason
    {
        public Reason()
        {
            this.FK_Request_DirectManager_Reason = new HashSet<Request>();
            this.FK_Request_Employee_Reason = new HashSet<Request>();
            this.FK_Request_HR_Director_ReasonID = new HashSet<Request>();
            this.FK_Request_HR_ManagerOfPersonal_ReasonID = new HashSet<Request>();
            this.FK_Request_HR_PerformanceUnit_ReasonID = new HashSet<Request>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
    
        public virtual ICollection<Request> FK_Request_DirectManager_Reason { get; set; }
        public virtual ICollection<Request> FK_Request_Employee_Reason { get; set; }
        public virtual ICollection<Request> FK_Request_HR_Director_ReasonID { get; set; }
        public virtual ICollection<Request> FK_Request_HR_ManagerOfPersonal_ReasonID { get; set; }
        public virtual ICollection<Request> FK_Request_HR_PerformanceUnit_ReasonID { get; set; }
    }
}