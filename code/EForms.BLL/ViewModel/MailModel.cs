using System.Net.Mail;

namespace Performance.Management.BLL.ViewModel
{
    public class MailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cs { get; set; }
        public string Bcs { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachment { get; set; }
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }
    }
}
