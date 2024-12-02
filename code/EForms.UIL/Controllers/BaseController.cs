using Performance.Management.BLL.Helper;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.MainEntities;
using Performance.Management.UIL.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Performance.Management.UIL.Controllers
{
    public class BaseController : Controller
    {
        readonly Permission _Permission = new Permission();
        readonly OasisSync _OasisSync = new OasisSync();

        public BaseController() { }

        public bool CheckCurrentUserIsHR => _Permission.CheckUserIsHR(GetUserName());
      
        public bool IsAllowedUser()
        {
            string userName = GetUserName();

            bool hrPerformanceUntiUser = new Entities().HRSAllowedUsers.Any(x => x.UserName == userName && x.IsActive == true);

            return hrPerformanceUntiUser;
        }

        public string GetUserName()
        {           
             return "1077732913";// return "malagaleen" Moajep;
            //  return "1016079509"; //ALI SALEH  AL DUKHAIL
            // return "1095992796"; //Hisham
            // return "1071165433"; //afnan hr
            // return "1006414922"; //mansour hr
           // return "1095135511"; //FAISAL SALEH AL HIJJI

            if (Environment.MachineName.Contains("MONIB"))
            {
                
            }

            WindowsIdentity identity = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            var id = identity.Name.Substring(identity.Name.IndexOf(@"\") + 1, identity.Name.Length - 5);

            if (id == "2545934511")
            {
                return "1071165433"; //afnan hr
            }

            return id;
        }

        public string GetCurrentLanguage()
        {
            return Request?.Cookies["culture"]?.Value ?? "en";
        }

        public string GetPreviousUrl() => System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            HttpCookie langCookie = Request.Cookies["culture"];
            string lang;
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = LanguageMang.GetDefaultLanguage();
                }
            }
            new LanguageMang().SetLanguage(lang);
            Session["Language"] = lang;

            return base.BeginExecuteCore(callback, state);
        }

        public Employee GetUserInfo()
        {
            OasisSync _OasisSync = new OasisSync();

            //Session["Employee"] = null;
            _ = new Employee();
            //WindowsIdentity identity = System.Web.HttpContext.Current.Request.LogonUserIdentity;

            Employee _Employee = _OasisSync.GetEmployeeByUserName(GetUserName());

            //Session["Employee"] = _Employee;
            return _Employee;
        }

        public bool CheckGenericUser()
        {
            ADSync _ADSync = new ADSync();
            return _ADSync.CheckADUserInGenericUser(GetUserName());
        }

        public void Success(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Success, AlertFas.Success, Captions.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Information, AlertFas.Information, Captions.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Danger, AlertFas.Warning, Language.Resource.Validation, message, dismissable);
        }

        public void Danger(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Danger, AlertFas.Danger, Captions.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string alertFa, string caption, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                AlertFa = alertFa,
                Caption = caption,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public List<EmpSearch> GetEmpList()
        {
            return _OasisSync.GetEmpList().Where(x => x.CONT_END_DATE >= DateTime.Now.Date && (x.LAST_WORKING_DATE == null || x.LAST_WORKING_DATE >= DateTime.Now.Date)).ToList();
        }
    }
}
