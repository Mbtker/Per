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
    
    public partial class Status
    {
        public Status()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public bool IsReject { get; set; }
        public bool IsPending { get; set; }
    
        public virtual ICollection<Request> Requests { get; set; }
    }
}