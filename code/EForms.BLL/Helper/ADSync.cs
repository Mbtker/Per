using Performance.Management.BLL.ViewModel;
using System;
using System.Configuration;
using System.DirectoryServices;

namespace Performance.Management.BLL.Helper
{
    public class ADSync
    {
        readonly HRService.HRClient _HRClient = new HRService.HRClient();
        readonly OasisSync _OasisSync = new OasisSync();

        public Employee GetEmployeeByUserName(string UserName)
        {
            Employee _Employee = new Employee();
            string Staff_ID = _HRClient.GetEmpIdwithAD(UserName);
            if (!String.IsNullOrEmpty(Staff_ID))
            {
                _Employee = _OasisSync.GetEmployeeByStaffID(Staff_ID);
            }
            _HRClient.Close();
            return _Employee;
        }

        public bool CheckADUserInGenericUser(string ADuserID)
        {
            try
            {
                DirectoryEntry rootDSE = new DirectoryEntry(ConfigurationManager.AppSettings["ADDomine"], ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
                DirectorySearcher urSearch = new DirectorySearcher(rootDSE, "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + ADuserID.ToLower().Trim() + "))"
                    , null, SearchScope.Subtree);
                SearchResult userProperties = urSearch.FindOne();
                if (userProperties != null)
                    return userProperties.Path.ToLower().Contains("GENERIC USERS".ToLower());
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
