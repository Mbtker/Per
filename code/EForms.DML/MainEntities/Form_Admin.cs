//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublicRelation.DML.MainEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Form_Admin
    {
        public int ID { get; set; }
        public Nullable<int> FormID { get; set; }
        public string AdminUserName { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
    
        public virtual Form Form { get; set; }
    }
}
