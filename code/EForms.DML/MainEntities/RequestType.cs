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
    
    public partial class RequestType
    {
        public RequestType()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int ID { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
    
        public virtual ICollection<Request> Requests { get; set; }
    }
}