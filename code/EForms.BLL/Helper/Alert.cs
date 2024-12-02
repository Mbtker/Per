namespace Performance.Management.BLL.Helper
{
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string AlertFa { get; set; }
        public string Caption { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

    public static class AlertFas
    {
        public const string Success = "fa-check";
        public const string Information = "fa-bell";
        public const string Warning = "fa-warning";
        public const string Danger = "fa-times";
    }

    public static class Captions
    {
        public const string Success = "Success";
        public const string Information = "Notification";
        public const string Warning = "Warning";
        public const string Danger = "Error";
    }
}
