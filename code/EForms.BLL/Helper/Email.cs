using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.MainEntities;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Performance.Management.BLL.Helper
{
    public class Email
    {
        public bool SendEmail(MailModel MailModel)
        {
            try
            {
                MailMessage mail = new MailMessage();

                string FromMail = ConfigurationManager.AppSettings["FromMail"];
                string FromName = ConfigurationManager.AppSettings["FromName"];
                string SMTP = ConfigurationManager.AppSettings["SMTP"];
                string SMTPUsername = ConfigurationManager.AppSettings["SMTP-Username"];
                string SMTPPassword = ConfigurationManager.AppSettings["SMTP-Password"];
                int SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP-Port"]);
                bool EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                mail.From = new MailAddress(FromMail, FromName);
                //mail.To.Add("whussien@pscc.med.sa");

                if (MailModel.To.Contains(";"))
                {
                    string[] toList = MailModel.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string to in toList)
                    {
                        mail.To.Add(to);
                    }
                }
                else
                {
                    mail.To.Add(MailModel.To);
                }

                //mail.CC.Add(MailModel.Cs);
                mail.Subject = MailModel.Subject;
                mail.Body = MailModel.Body;
                mail.IsBodyHtml = true;

                if (!String.IsNullOrEmpty(MailModel.Attachment))
                {
                    mail.Attachments.Add(new Attachment(MailModel.Attachment));
                }

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.None;
                //mail.Headers.Add("Disposition-Notification-To", "whussien@pscc.med.sa");
                //mail.Headers.Add("Disposition-Notification-To", FromMail);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = SMTP;
                smtp.Port = SMTPPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                smtp.EnableSsl = EnableSsl;
                smtp.Send(mail);

                //if (mail.DeliveryNotificationOptions == DeliveryNotificationOptions.OnSuccess)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public bool SendAdToAllEmployee(string message)
        {
            try
            {
                MailModel _MailModel = new MailModel();
                _MailModel.To = "whussien@pscc.med.sa";
                _MailModel.Cs = "whussien@pscc.med.sa";
                _MailModel.Subject = "New Job Advertisement";
                _MailModel.Body = message;
                //_MailModel.Attachment = @"D:\Wael\Projects\Internal Job Advertisment System Analysis and design.pdf";

                return SendEmail(_MailModel);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool SendTo(string Body, string To, string Subject)
        {
            try
            {
                MailModel _MailModel = new MailModel();
                _MailModel.To = To;
                _MailModel.Subject = Subject;
                _MailModel.Body = Body;

                return SendEmail(_MailModel);
            }
            catch (Exception)
            {
                return false;
            }
            //PORForms – Request No. 
        }


        public bool SendToNew(string Subject, string To, string StringName, string RequestNo, string Description, string Date, string Status)
        {
            try
            {
                MailModel _MailModel = new MailModel();
                _MailModel.To = To;
                _MailModel.Subject = "Performance Management System - Request For " + Subject;
                //_MailModel.Subject = "PORForms – Request No. " + RequestNo;
                _MailModel.Body = GetNewTemplate(StringName, RequestNo, Description, Date, Status);

                return SendEmail(_MailModel);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendToApprove(string Subject, string To, string Position, Status _Status, Request _Request)
        {
            try
            {
                MailModel _MailModel = new MailModel();
                _MailModel.To = To;
                _MailModel.Subject = "Performance Management System - Need Your Approvals For " + Subject;
                _MailModel.Body = GetApproveTemplate(Position, _Status,_Request);

                return SendEmail(_MailModel);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SendToClosedReject(string Subject, string To, string StringName, string RequestNo, string EnRejectClosed, string ArRejectClosed, string UserFullName, string Date, string Status, string Note)
        {
            try
            {
                MailModel _MailModel = new MailModel();
                _MailModel.To = To;
                _MailModel.Subject = "Performance Management System - Request For " + Subject;
                _MailModel.Body = GetClosedRejectTemplate(StringName, RequestNo, EnRejectClosed, ArRejectClosed, UserFullName, Date, Status, Note);

                return SendEmail(_MailModel);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetNewTemplate(string StringName, string RequestNo, string Description, string Date, string Status)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/Content/EmailTemplates/NewEmailTemplate.html");
            string body = File.ReadAllText(filePath, Encoding.UTF8);
            body = body.Replace("{0}", StringName);
            body = body.Replace("{1}", RequestNo);
            body = body.Replace("{2}", "New Request");
            body = body.Replace("{3}", "New Request");
            body = body.Replace("{4}", Status);
            //body = body.Replace("{5}", Group);
            body = body.Replace("{6}", Date);

            return body;
        }

        public string GetApproveTemplate(string Position, Status _Status, Request _Request)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/Content/EmailTemplates/ApproveTemplate.html");
            string body = File.ReadAllText(filePath, Encoding.UTF8);
            body = body.Replace("{0}", Position);
            body = body.Replace("{1}", _Request.ID.ToString());
            body = body.Replace("{2}", _Request.RequesterUserName);
            body = body.Replace("{3}", _Request.RequesterUserEnglishName);
            body = body.Replace("{4}", _Request.RequesterDeptEnglishName);
            body = body.Replace("{5}", _Status.Name);
            body = body.Replace("{55}", _Status.NameAr);

            StringBuilder RequestDetails = new StringBuilder();
            _ = RequestDetails.Append("<table style='text-align:center;' class='RequestDetails'>");
            _ = RequestDetails.Append(@"<tr>
                                        <td style='text-align:left'>Request Number</td>
                                        <td style='text-align: center'>
                                            <span>" + _Request.ID + "</span>" +
                                        @"</td>
                                        <td style='text-align: right'>رقم الطلب</td>
                                    </tr>");

            //if (_Request.FormType != null)
            //{
            //    _ = RequestDetails.Append(@"<tr>
            //                            <td style='text-align:left'>Request Type</td>
            //                            <td style='text-align: center'>
            //                                <span>" + ((_Request.FormType != null) ? _Request.FormType.Name : "") + "</span>" +
            //                               @"</td>
            //                            <td style='text-align: right'>نوع الطلب</td>
            //                        </tr>");
            //}

            _ = RequestDetails.Append(@"</table>");

            body = body.Replace("{6}", RequestDetails.ToString());

            return body;
        }

        public string GetClosedRejectTemplate(string StringName, string RequestNo, string EnRejectClosed, string ArRejectClosed, string UserFullName, string Date, string Status, string Note)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/Content/EmailTemplates/RejectClosedEmailTemplate.html");
            string body = File.ReadAllText(filePath, Encoding.UTF8);
            body = body.Replace("{0}", StringName);
            body = body.Replace("{1}", RequestNo);
            body = body.Replace("{2}", EnRejectClosed);
            body = body.Replace("{3}", ArRejectClosed);
            body = body.Replace("{4}", EnRejectClosed + " Request");
            body = body.Replace("{5}", Status);
            body = body.Replace("{6}", Date);
            body = body.Replace("{7}", Note);

            return body;
        }

    }
}
