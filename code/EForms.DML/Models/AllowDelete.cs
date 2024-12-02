namespace Performance.Management.DML.Models
{
    public class AllowDelete
    {
        public int RequestID { get; set; }
        public bool IsRequestUserName { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAction { get; set; }
        public bool IsFinished { get; set; }
        public string NotAppied_STAFF_ID { get; set; }
    }
}
