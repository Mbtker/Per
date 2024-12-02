using Newtonsoft.Json;
using Performance.Management.BLL.ViewModel;
using Performance.Management.DML.PSCCSetting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Performance.Management.BLL.Helper
{
    public class OasisSync
    {
        readonly InMemoryCache memoryCache = new InMemoryCache();

        readonly HRService.HRClient _HRClient = new HRService.HRClient();
        readonly QryExe.QryExeClient QryExe = new QryExe.QryExeClient();
        readonly PSCCSetting PSCCSettingDB = new PSCCSetting();

        #region Oasis Beside Issues

        public void FillDepartmentHeadTable()
        {
            foreach (var DepartmentHead in PSCCSettingDB.DepartmentHeads)
            {
                string STAFF_ID = DepartmentHead.DepartmentHeadStaffID.ToString();
                _ = new Employee();
                Employee _Employee = GetEmployeeByStaffID(STAFF_ID);

                PSCCSetting _PSCCSettingDB = new PSCCSetting();
                DepartmentHead _DepartmentHead = _PSCCSettingDB.DepartmentHeads.Find(DepartmentHead.ID);

                if (_DepartmentHead != null && _Employee != null)
                {
                    _DepartmentHead.DepartmentHeadUserName = _Employee.USER_NAME;
                    _DepartmentHead.DepartmentHeadFullName = _Employee.ENGL_STAFF_NAME;
                    _DepartmentHead.DepartmentHeadPositionName = _Employee.POSITION_ENGLISH;
                }

                _PSCCSettingDB.Entry(_DepartmentHead).State = EntityState.Modified;
                _ = _PSCCSettingDB.SaveChanges();
            }
        }

        #endregion

        #region Employee Helper

        public Employee GetEmployeeByStaffID(string Staff_ID)
        {
            var emp = memoryCache.GetOrSet($"GetEmployeeByStaffID{Staff_ID}", () => GetEmpByStaffID(Staff_ID));

            return emp;
        }

        private Employee GetEmpByStaffID(string Staff_ID)
        {
            string Query = ConfigurationManager.AppSettings["EmployeeQry"] + " AND I.staff_id = '" + Staff_ID + "' ORDER BY I.ENGL_STAFF_NAME";

            DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            Employee _Employee = new Employee();

            if (tbEmployee != null && tbEmployee.Rows.Count > 0)
            {
                _Employee.DOC_NUMBER = tbEmployee.Rows[0]["DOC_NUMBER"].ToString();

                /////////**************************************************************************/
                //////if (tbEmployee.Rows[0]["HeadDeptCode"].ToString() == "")
                //////    _Employee.WORK_ENTITY = tbEmployee.Rows[0]["deptcd"].ToString();
                //////else
                //////    _Employee.WORK_ENTITY = tbEmployee.Rows[0]["HeadDeptCode"].ToString();
                /////////**************************************************************************/



                _Employee.WORK_ENTITY = tbEmployee.Rows[0]["WORK_ENTITY"].ToString();
                //_Employee.AMOUNT = tbEmployee.Rows[0]["AMOUNT"].ToString();
                _Employee.ARB_STAFF_NAME = tbEmployee.Rows[0]["ARB_STAFF_NAME"].ToString();
                //_Employee.BENEFITS = tbEmployee.Rows[0]["BENEFITS"].ToString();
                _Employee.CONT_END_DATE_ARAB = tbEmployee.Rows[0]["CONT_END_DATE_ARAB"].ToString();
                _Employee.CONT_END_DATE_ENG = tbEmployee.Rows[0]["CONT_END_DATE_ENG"].ToString();
                _Employee.CONT_START_DATE_ARAB = tbEmployee.Rows[0]["CONT_START_DATE_ARAB"].ToString();
                _Employee.CONT_START_DATE_ENG = tbEmployee.Rows[0]["CONT_START_DATE_ENG"].ToString();
                _Employee.CONTRACT_STATUS_ARAB = tbEmployee.Rows[0]["CONTRACT_STATUS_ARAB"].ToString();
                _Employee.CONTRACT_STATUS_ENG = tbEmployee.Rows[0]["CONTRACT_STATUS_ENG"].ToString();
                _Employee.CONTRACT_TYPE_ARAB = tbEmployee.Rows[0]["CONTRACT_TYPE_ARAB"].ToString();
                _Employee.CONTRACT_TYPE_ENG = tbEmployee.Rows[0]["CONTRACT_TYPE_ENG"].ToString();
                _Employee.CTYPE = tbEmployee.Rows[0]["CTYPE"].ToString();
                _Employee.DESCRIPTION = tbEmployee.Rows[0]["DESCRIPTION"].ToString();

                /***************************************************************************************/
                //if (tbEmployee.Rows[0]["HeadDept"].ToString() == "")
                //{
                //    _Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["DEPARTMENT_ARABIC"].ToString();
                //    _Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["DEPARTMENT_ENGLISH"].ToString();
                //}
                //else
                //{
                //    _Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["HeadDept"].ToString();
                //    _Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["HeadDept"].ToString();
                //}                    
                _Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["DEPARTMENT_ARABIC"].ToString();
                _Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["DEPARTMENT_ENGLISH"].ToString();
                /****************************************************************************************/

                _Employee.EMAIL_ADDRESS = tbEmployee.Rows[0]["EMAIL_ADDRESS"].ToString();
                _Employee.USER_NAME = (tbEmployee.Rows[0]["EMAIL_ADDRESS"].ToString()).Split('@')[0];
                _Employee.ENGL_STAFF_NAME = tbEmployee.Rows[0]["ENGL_STAFF_NAME"].ToString();
                //_Employee.GRADE_STEP = tbEmployee.Rows[0]["GRADE_STEP"].ToString();
                _Employee.HIRING_ARAB = tbEmployee.Rows[0]["HIRING_ARAB"].ToString();
                _Employee.HIRING_ENG = tbEmployee.Rows[0]["HIRING_ENG"].ToString();
                _Employee.NAT_ARAB = tbEmployee.Rows[0]["NAT_ARAB"].ToString();
                _Employee.NAT_ENG = tbEmployee.Rows[0]["NAT_ENG"].ToString();
                _Employee.PATIENT_NO = tbEmployee.Rows[0]["PATIENT_NO"].ToString();
                _Employee.POSITION_ARABIC = tbEmployee.Rows[0]["POSITION_ARABIC"].ToString();
                _Employee.POSITION_ENGLISH = tbEmployee.Rows[0]["POSITION_ENGLISH"].ToString();
                _Employee.SEX = tbEmployee.Rows[0]["SEX"].ToString();
                _Employee.STAFF_ID = tbEmployee.Rows[0]["STAFF_ID"].ToString();
                //_Employee.STAFF_ID1 = tbEmployee.Rows[0]["STAFF_ID1"].ToString();
                //_Employee.STAFF_ID2 = tbEmployee.Rows[0]["STAFF_ID2"].ToString();
                _Employee.POSITION_TYPE = tbEmployee.Rows[0]["POSITION_TYPE"].ToString();

                //QryExe.Close();
            }

            return _Employee;
        }

        public Employee GetEmployeeByIDentityNo(string IdentityNo)
        {
            var emp = memoryCache.GetOrSet($"GetEmployeeByIDentityNo{IdentityNo}", () => GetEmpByNationalID(IdentityNo));

            return emp;
        }

        private Employee GetEmpByNationalID(string IdentityNo)
        {
            string Query = string.Format(ConfigurationManager.AppSettings["EmployeeQryByID"], IdentityNo);

            DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            Employee _Employee = new Employee();

            if (tbEmployee != null && tbEmployee.Rows.Count > 0)
            {
                _Employee.DOC_NUMBER = tbEmployee.Rows[0]["DOC_NUMBER"].ToString();

                /**************************************************************************/
                if (tbEmployee.Rows[0]["HeadDeptCode"].ToString() == "")
                    _Employee.WORK_ENTITY = tbEmployee.Rows[0]["WORK_ENTITY"].ToString();
                else
                    _Employee.WORK_ENTITY = tbEmployee.Rows[0]["HeadDeptCode"].ToString();
                /**************************************************************************/



                //_Employee.WORK_ENTITY = tbEmployee.Rows[0]["WORK_ENTITY"].ToString();
                //_Employee.AMOUNT = tbEmployee.Rows[0]["AMOUNT"].ToString();
                _Employee.ARB_STAFF_NAME = tbEmployee.Rows[0]["ARB_STAFF_NAME"].ToString();
                //_Employee.BENEFITS = tbEmployee.Rows[0]["BENEFITS"].ToString();
                _Employee.CONT_END_DATE_ARAB = tbEmployee.Rows[0]["CONT_END_DATE_ARAB"].ToString();
                _Employee.CONT_END_DATE_ENG = tbEmployee.Rows[0]["CONT_END_DATE_ENG"].ToString();
                _Employee.CONT_START_DATE_ARAB = tbEmployee.Rows[0]["CONT_START_DATE_ARAB"].ToString();
                _Employee.CONT_START_DATE_ENG = tbEmployee.Rows[0]["CONT_START_DATE_ENG"].ToString();
                _Employee.CONTRACT_STATUS_ARAB = tbEmployee.Rows[0]["CONTRACT_STATUS_ARAB"].ToString();
                _Employee.CONTRACT_STATUS_ENG = tbEmployee.Rows[0]["CONTRACT_STATUS_ENG"].ToString();
                _Employee.CONTRACT_TYPE_ARAB = tbEmployee.Rows[0]["CONTRACT_TYPE_ARAB"].ToString();
                _Employee.CONTRACT_TYPE_ENG = tbEmployee.Rows[0]["CONTRACT_TYPE_ENG"].ToString();
                _Employee.CTYPE = tbEmployee.Rows[0]["CTYPE"].ToString();
                _Employee.DESCRIPTION = tbEmployee.Rows[0]["DESCRIPTION"].ToString();

                /***************************************************************************************/
                if (tbEmployee.Rows[0]["HeadDept"].ToString() == "")
                {
                    _Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["DEPARTMENT_ARABIC"].ToString();
                    _Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["DEPARTMENT_ENGLISH"].ToString();
                }
                else
                {
                    _Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["HeadDept"].ToString();
                    _Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["HeadDept"].ToString();
                }
                //_Employee.DEPARTMENT_ARABIC = tbEmployee.Rows[0]["DEPARTMENT_ARABIC"].ToString();
                //_Employee.DEPARTMENT_ENGLISH = tbEmployee.Rows[0]["DEPARTMENT_ENGLISH"].ToString();
                /****************************************************************************************/

                _Employee.EMAIL_ADDRESS = tbEmployee.Rows[0]["EMAIL_ADDRESS"].ToString();
                //_Employee.USER_NAME = (tbEmployee.Rows[0]["EMAIL_ADDRESS"].ToString()).Split('@')[0];
                _Employee.USER_NAME = tbEmployee.Rows[0]["DOC_NUMBER"].ToString();
                _Employee.ENGL_STAFF_NAME = tbEmployee.Rows[0]["ENGL_STAFF_NAME"].ToString();
                _Employee.GRADE_STEP = tbEmployee.Rows[0]["GRADE_STEP"].ToString();
                _Employee.HIRING_ARAB = tbEmployee.Rows[0]["HIRING_ARAB"].ToString();
                _Employee.HIRING_ENG = tbEmployee.Rows[0]["HIRING_ENG"].ToString();
                _Employee.NAT_ARAB = tbEmployee.Rows[0]["NAT_ARAB"].ToString();
                _Employee.NAT_ENG = tbEmployee.Rows[0]["NAT_ENG"].ToString();
                _Employee.PATIENT_NO = tbEmployee.Rows[0]["PATIENT_NO"].ToString();
                _Employee.POSITION_ARABIC = tbEmployee.Rows[0]["POSITION_ARABIC"].ToString();
                _Employee.POSITION_ENGLISH = tbEmployee.Rows[0]["POSITION_ENGLISH"].ToString();
                _Employee.SEX = tbEmployee.Rows[0]["SEX"].ToString();
                _Employee.STAFF_ID = tbEmployee.Rows[0]["STAFF_ID"].ToString();
                //_Employee.STAFF_ID1 = tbEmployee.Rows[0]["STAFF_ID1"].ToString();
                //_Employee.STAFF_ID2 = tbEmployee.Rows[0]["STAFF_ID2"].ToString();
                _Employee.POSITION_TYPE = tbEmployee.Rows[0]["POSITION_TYPE"].ToString();

                //QryExe.Close();
            }

            return _Employee;
        }

        public Employee GetEmployeeByUserName(string UserName)
        {
            var _Employee = memoryCache.GetOrSet($"GetEmployeeByUserName{UserName}", () => GetEmployeeByIDentityNo(UserName));

            /*string Staff_ID = _HRClient.GetEmpIdwithAD(UserName);
            if (!String.IsNullOrEmpty(Staff_ID))
            {             
                _Employee = GetEmployeeByStaffID(Staff_ID);
            }*/

            return _Employee;
        }

        /*  public Employee GetEmployeeByUserName(string UserName)
          {
              Employee _Employee = new Employee();
              string Staff_ID = _HRClient.GetEmpIdwithAD(UserName);
              if (!String.IsNullOrEmpty(Staff_ID))
              {
                  _Employee = GetEmployeeByStaffID(Staff_ID);
              }
              return _Employee;
          }
          */
        public List<Employee> GetAllEmployeeByDeptCode(string DeptCode)
        {
            var employees = memoryCache.GetOrSet($"GetAllEmployeeByDeptCode{DeptCode}", () => GetEmployeesByDeptCode(DeptCode));

            return employees;
        }

        private List<Employee> GetEmployeesByDeptCode(string DeptCode)
        {
            string Query = ConfigurationManager.AppSettings["EmployeeQry"] + " AND (i.work_entity = '" + DeptCode + "' or a.deptcd = '" + DeptCode + "' or b.CURRENTDEPTCODE = '" + DeptCode + "')";
            //string Query = ConfigurationManager.AppSettings["EmployeeQry"] + " AND (work_entity = '" + DeptCode + "' )";
            DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            List<Employee> EmployeeList = new List<Employee>();

            if (tbEmployee != null && tbEmployee.Rows.Count > 0)
            {

                foreach (DataRow Row in tbEmployee.Rows)
                {
                    Employee _Employee = new Employee();

                    _Employee.DOC_NUMBER = Row["DOC_NUMBER"].ToString();
                    _Employee.WORK_ENTITY = Row["WORK_ENTITY"].ToString();
                    //_Employee.AMOUNT = Row["AMOUNT"].ToString();
                    _Employee.ARB_STAFF_NAME = Row["ARB_STAFF_NAME"].ToString();
                    //_Employee.BENEFITS = Row["BENEFITS"].ToString();
                    _Employee.CONT_END_DATE_ARAB = Row["CONT_END_DATE_ARAB"].ToString();
                    _Employee.CONT_END_DATE_ENG = Row["CONT_END_DATE_ENG"].ToString();
                    _Employee.CONT_START_DATE_ARAB = Row["CONT_START_DATE_ARAB"].ToString();
                    _Employee.CONT_START_DATE_ENG = Row["CONT_START_DATE_ENG"].ToString();
                    _Employee.CONTRACT_STATUS_ARAB = Row["CONTRACT_STATUS_ARAB"].ToString();
                    _Employee.CONTRACT_STATUS_ENG = Row["CONTRACT_STATUS_ENG"].ToString();
                    _Employee.CONTRACT_TYPE_ARAB = Row["CONTRACT_TYPE_ARAB"].ToString();
                    _Employee.CONTRACT_TYPE_ENG = Row["CONTRACT_TYPE_ENG"].ToString();
                    _Employee.CTYPE = Row["CTYPE"].ToString();
                    _Employee.DESCRIPTION = Row["DESCRIPTION"].ToString();
                    _Employee.DEPARTMENT_ARABIC = Row["DEPARTMENT_ARABIC"].ToString();
                    _Employee.DEPARTMENT_ENGLISH = Row["DEPARTMENT_ENGLISH"].ToString();
                    _Employee.EMAIL_ADDRESS = Row["EMAIL_ADDRESS"].ToString();
                    _Employee.USER_NAME = (Row["EMAIL_ADDRESS"].ToString()).Split('@')[0];
                    _Employee.ENGL_STAFF_NAME = Row["ENGL_STAFF_NAME"].ToString();
                    //_Employee.GRADE_STEP = Row["GRADE_STEP"].ToString();
                    _Employee.HIRING_ARAB = Row["HIRING_ARAB"].ToString();
                    _Employee.HIRING_ENG = Row["HIRING_ENG"].ToString();
                    _Employee.NAT_ARAB = Row["NAT_ARAB"].ToString();
                    _Employee.NAT_ENG = Row["NAT_ENG"].ToString();
                    _Employee.PATIENT_NO = Row["PATIENT_NO"].ToString();
                    _Employee.POSITION_ARABIC = Row["POSITION_ARABIC"].ToString();
                    _Employee.POSITION_ENGLISH = Row["POSITION_ENGLISH"].ToString();
                    _Employee.SEX = Row["SEX"].ToString();
                    _Employee.STAFF_ID = Row["STAFF_ID"].ToString();
                    //_Employee.STAFF_ID1 = Row["STAFF_ID1"].ToString();
                    //_Employee.STAFF_ID2 = Row["STAFF_ID2"].ToString();

                    EmployeeList.Add(_Employee);
                }
            }

            return EmployeeList;
        }

        public List<OasisPosition> GetOasisPositions()
        {
            var positionList = memoryCache.GetOrSet($"GetOasisPositionList", () => GetOasisPositionList(), minutes: 5);

            return positionList;
        }

        private List<OasisPosition> GetOasisPositionList()
        {
            string Query = ConfigurationManager.AppSettings["PositionListQry"];

            DataTable tbPosition = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            List<OasisPosition> EmployeeList = new List<OasisPosition>();

            if (tbPosition != null && tbPosition.Rows.Count > 0)
            {

                foreach (DataRow Row in tbPosition.Rows)
                {
                    OasisPosition _oasisPosition = new OasisPosition();

                    _oasisPosition.Code = Row["POSITION_TYPE"].ToString();
                    _oasisPosition.Tittle = Row["JOB_TITLE"].ToString();

                    EmployeeList.Add(_oasisPosition);
                }
            }

            return EmployeeList;
        }

        public List<OasisDepartment> GetOasisDepartments()
        {
            var deptList = memoryCache.GetOrSet($"GetOasisDepartmentList", () => GetOasisDepartmentList(), minutes: 5);

            return deptList;
        }

        private List<OasisDepartment> GetOasisDepartmentList()
        {
            string Query = ConfigurationManager.AppSettings["DeptListQry"];

            DataTable tbDept = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            List<OasisDepartment> EmployeeList = new List<OasisDepartment>();

            if (tbDept != null && tbDept.Rows.Count > 0)
            {

                foreach (DataRow Row in tbDept.Rows)
                {
                    OasisDepartment _oasisDept = new OasisDepartment();

                    _oasisDept.Code = Row["WORK_ENTITY"].ToString();
                    _oasisDept.Tittle = Row["DEPT_NAME"].ToString();

                    EmployeeList.Add(_oasisDept);
                }
            }

            return EmployeeList;
        }

        public OasisPosition GetOasisProductCodeById(string id)
        {
            string Query = ConfigurationManager.AppSettings["ProductQry"] + $" where doc_no = '{id}'";

            DataTable tbProduct = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            OasisPosition _oasisProduct = null;

            if (tbProduct != null && tbProduct.Rows.Count > 0)
            {
                foreach (DataRow Row in tbProduct.Rows)
                {
                    _oasisProduct = new OasisPosition();
                    _oasisProduct.Code = Row["ID"].ToString();
                    _oasisProduct.Tittle = Row["Name"].ToString();

                    return _oasisProduct;
                }
            }

            return _oasisProduct;
        }

        public Employee GetCurrentDepartmentHeadByEmpStaffID(string STAFFID)
        {
            if (!String.IsNullOrEmpty(STAFFID))
            {
                Employee _DepartmentHead = GetDepartmentHeadByEmpStaffID(STAFFID);
                if (_DepartmentHead != null)
                {
                    Employee _DelegatedHead = GetDelegatedHeadByDeptCode(_DepartmentHead.STAFF_ID);
                    if (!String.IsNullOrEmpty(_DelegatedHead.STAFF_ID))
                    {
                        return _DelegatedHead;
                    }
                    else
                    {
                        return _DepartmentHead;
                    }
                }
            }
            return null;
        }

        public Employee GetCurrentDepartmentHeadByDeptCode(string DeptCode)
        {
            if (!String.IsNullOrEmpty(DeptCode))
            {
                Employee _DepartmentHead = GetDepartmentHeadByDeptCode(DeptCode);
                if (_DepartmentHead != null && !string.IsNullOrWhiteSpace(_DepartmentHead.STAFF_ID))
                {
                    Employee _DelegatedHead = GetDelegatedHeadByDeptCode(_DepartmentHead.STAFF_ID);
                    if (!String.IsNullOrEmpty(_DelegatedHead.STAFF_ID))
                    {
                        return _DelegatedHead;
                    }
                    else
                    {
                        return _DepartmentHead;
                    }
                }
            }
            return null;
        }

        public Employee GetDepartmentHeadByEmpStaffID(string STAFFID)
        {
            Employee _Employee = new Employee();

            if (!String.IsNullOrEmpty(STAFFID))
            {
                string Query = ConfigurationManager.AppSettings["DepartmentHeadByEmpStaffIDQry"] + " WHERE a.staff_id = '" + STAFFID + "' and c.language_id = '1'";
                DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

                if (tbEmployee != null && tbEmployee.Rows.Count > 0)
                {
                    _Employee = GetEmployeeByStaffID(tbEmployee.Rows[0]["HeadStaffID"].ToString());
                }
            }

            return _Employee;
        }

        public Employee GetDepartmentHeadByDeptCode(string DeptCode)
        {
            Employee _Employee = new Employee();

            if (!String.IsNullOrEmpty(DeptCode))
            {
                string Query = ConfigurationManager.AppSettings["DepartmentHeadByDeptCodeQry"] + " WHERE a.DeptCode  LIKE '%" + DeptCode + "%'";
                DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];
                if (tbEmployee != null && tbEmployee.Rows.Count > 0)
                {
                    _Employee = GetEmployeeByStaffID(tbEmployee.Rows[0]["STAFFID"].ToString());
                }
            }

            return _Employee;
        }

        public Employee GetDelegatedHeadByDeptCode(string Staff_ID)
        {
            Employee _Employee = new Employee();

            if (!String.IsNullOrEmpty(Staff_ID))
            {
                string Query = ConfigurationManager.AppSettings["DelegationQry"] + " WHERE to_Date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/MM/yyyy') between datefrom and dateto AND ASSIGNEDBY  = '" + Staff_ID + "'";
                DataTable tbEmployee = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];
                if (tbEmployee != null && tbEmployee.Rows.Count > 0)
                {
                    _Employee = GetEmployeeByStaffID(tbEmployee.Rows[0]["ASSIGNEDTO"].ToString());
                }
            }

            return _Employee;
        }

        public Employee _GetDepartmentHeadByDeptCode(string DeptCode)
        {
            Employee _Employee = new Employee();

            if (!String.IsNullOrEmpty(DeptCode))
            {
                //DepartmentHead _DepartmentHead = PSCCSettingDB.DepartmentHeads.Where(d => d.DepartmentCode == DeptCode).SingleOrDefault();

                //if (_DepartmentHead != null && _DepartmentHead.DirectorType != null)
                //{
                //    if (_DepartmentHead.DirectorTypeID == 1)
                //    {
                //        _Employee = GetEmployeeByStaffID(_DepartmentHead.DepartmentHeadStaffID);
                //    }
                //    else if (_DepartmentHead.DirectorTypeID == 2)
                //    {
                //        _Employee = GetEmployeeByStaffID(_DepartmentHead.DepartmentDeputyDirectorStaffID);
                //    }
                //    else if (_DepartmentHead.DirectorTypeID == 3)
                //    {
                //        _Employee = GetEmployeeByStaffID(_DepartmentHead.DepartmentActingDirectorStaffID);
                //    }
                //}

            }

            return _Employee;
        }

        public string GetEmployeeGrade(string Staff_ID)
        {
            string grade = _HRClient.GetEmployeeGrade(Staff_ID);

            return grade;
        }

        #endregion

        #region Department Helper

        public List<Department> GetAllDepartment()
        {
            DataTable tbDepartment = _HRClient.GetallDeptList().Tables[0];

            List<Department> DepartmentList = new List<Department>();

            if (tbDepartment != null && tbDepartment.Rows.Count > 0)
            {

                foreach (DataRow Row in tbDepartment.Rows)
                {
                    Department _Department = new Department();

                    _Department.DEPT_NAME_AR = Row["DEPT_NAME_AR"].ToString();
                    _Department.DEPT_NAME_EN = Row["DEPT_NAME_EN"].ToString();
                    _Department.WORK_ENTITY = Row["WORK_ENTITY"].ToString();

                    DepartmentList.Add(_Department);
                }
            }
            return DepartmentList;
        }

        public Department GetDepartmentByCode(string DeptCode)
        {
            DataTable tbDepartment = _HRClient.GetallDeptList().Tables[0];

            Department _Department = new Department();

            if (tbDepartment != null && tbDepartment.Rows.Count > 0)
            {
                foreach (DataRow Row in tbDepartment.Rows)
                {
                    if (Row["WORK_ENTITY"].ToString() == DeptCode)
                    {
                        _Department.DEPT_NAME_AR = Row["DEPT_NAME_AR"].ToString();
                        _Department.DEPT_NAME_EN = Row["DEPT_NAME_EN"].ToString();
                        _Department.WORK_ENTITY = Row["WORK_ENTITY"].ToString();
                    }
                }
            }
            return _Department;
        }

        #endregion

        #region Position Helper

        public List<PositionType> GetAllPositionTypes()
        {
            var types = memoryCache.GetOrSet($"GetAllPositionTypes", () => GetAllPositionTypeList());

            return types;
        }

        private List<PositionType> GetAllPositionTypeList()
        {
            string Query = ConfigurationManager.AppSettings["GetAllPositionsQry"];
            DataTable tbPositionTypes = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            List<PositionType> PositionTypeList = new List<PositionType>();

            if (tbPositionTypes != null && tbPositionTypes.Rows.Count > 0)
            {

                foreach (DataRow Row in tbPositionTypes.Rows)
                {
                    PositionType _PositionType = new PositionType
                    {
                        position_type = Row["position_type"].ToString(),
                        description = Row["description"].ToString()
                    };

                    PositionTypeList.Add(_PositionType);
                }
            }
            return PositionTypeList;
        }

        public string GetPositionByType(string Type)
        {
            string description = "";

            string Query = ConfigurationManager.AppSettings["GetAllPositionsQry"] + " AND position_type = '" + Type + "'";
            DataTable tbPositionTypes = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

            if (tbPositionTypes != null && tbPositionTypes.Rows.Count > 0)
            {
                description = tbPositionTypes.Rows[0]["description"].ToString();
            }
            return description;
        }

        #endregion

        #region GetJobDetails

        //public List<JobDetail> GetAllJobDetails()
        //{
        //    var types = memoryCache.GetOrSet($"GetAllJobDetails", () => GetAllJobDetailList());

        //    return types;
        //}

        //private List<JobDetail> GetAllJobDetailList()
        //{
        //    string Query = ConfigurationManager.AppSettings["GetAllPositionsQry"];
        //    DataTable tbJobDetails = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

        //    List<JobDetail> JobDetailList = new List<JobDetail>();

        //    if (tbJobDetails != null && tbJobDetails.Rows.Count > 0)
        //    {

        //        foreach (DataRow Row in tbJobDetails.Rows)
        //        {
        //            JobDetail _JobDetail = new JobDetail
        //            {
        //                position_type = Row["position_type"].ToString(),
        //                description = Row["description"].ToString()
        //            };

        //            JobDetailList.Add(_JobDetail);
        //        }
        //    }
        //    return JobDetailList;
        //}

        //public string GetPositionByType(string Type)
        //{
        //    string description = "";

        //    string Query = ConfigurationManager.AppSettings["GetAllPositionsQry"] + " AND position_type = '" + Type + "'";
        //    DataTable tbJobDetails = QryExe.ExecuteSelectSQL(ConfigurationManager.AppSettings["DBUsername"], ConfigurationManager.AppSettings["DBPassword"], ConfigurationManager.AppSettings["DBtns"], Query).Tables[0];

        //    if (tbJobDetails != null && tbJobDetails.Rows.Count > 0)
        //    {
        //        description = tbJobDetails.Rows[0]["description"].ToString();
        //    }
        //    return description;
        //}

        #endregion

        #region ReportingToEmployees

        public List<Employee> GetMyReportingEmployeesByIDentityNo(string IdentityNo)
        {
            var empList = memoryCache.GetOrSet($"GetEmployeeListByIDentityNo{IdentityNo}", () => GetMyReportingEmployeesByNationalID(IdentityNo));

            return empList;
        }

        private List<Employee> GetMyReportingEmployeesByNationalID(string IdentityNo)
        {
            string baseUri = string.Format(ConfigurationManager.AppSettings["HrApiUri"], IdentityNo);
            string requestUri = string.Format(ConfigurationManager.AppSettings["GetMyReportingEmployees"], IdentityNo);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(requestUri: requestUri).Result;
            response.EnsureSuccessStatusCode();

            // De-serialize the updated product from the response body.
            var json = response.Content.ReadAsStringAsync().Result;

            var useres = JsonConvert.DeserializeObject<List<OasisUser>>(json);

            var empList = new List<Employee>();

            foreach (var user in useres)
            {
                Employee employee = new Employee()
                {
                    ARB_STAFF_NAME = user.StaffNameAr,
                    ENGL_STAFF_NAME = user.StaffNameEn,
                    CONTRACT_STATUS_ARAB = user.ContractStatusAr,
                    CONTRACT_STATUS_ENG = user.ContractStatusEn,
                    CONTRACT_TYPE_ARAB = user.ContractTypeAr,
                    CONTRACT_TYPE_ENG = user.ContractTypeEn.Trim(),
                    CONT_END_DATE_ARAB = user.ContEndDateAr,
                    CONT_END_DATE_ENG = user.ContEndDateEn,
                    CONT_START_DATE_ARAB = user.ContStartDateAr,
                    CONT_START_DATE_ENG = user.ContStartDateEn,
                    DEPARTMENT_ARABIC = user.DepartmentAr,
                    DEPARTMENT_ENGLISH = user.DepartmentEn,
                    DESCRIPTION = user.Description,
                    DOC_NUMBER = user.DocNumber,
                    EMAIL_ADDRESS = user.Email,
                    HIRING_ARAB = user.HiringDateAr,
                    HIRING_ENG = user.HiringDateEn,
                    SEX = user.Sex,
                    NAT_ARAB = user.NatAr,
                    NAT_ENG = user.NatEn,
                    POSITION_ARABIC = user.PositionAr,
                    POSITION_ENGLISH = user.PositionEn,
                    POSITION_TYPE = user.PositionType,
                    STAFF_ID = user.StaffId,
                    USER_NAME = user.DocNumber,
                    WORK_ENTITY = user.WorkEntity
                };

                empList.Add(employee);
            }

            //return empList.Where(x => !x.CONTRACT_TYPE_ENG.ToLower().Contains("msd")).ToList();
            return empList.Where(d => d.CONTRACT_TYPE_ENG.Equals("INTERNATIONAL", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_ENG.Equals("LOCAL (40 DAYS)", StringComparison.OrdinalIgnoreCase) || d.CONTRACT_TYPE_ENG.Equals("LOCAL (30 DAYS)", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public OasisUser GetEmployeeByNationalIdOrStaffId(string id)
        {
            string baseUri = ConfigurationManager.AppSettings["HrApiUri"];
            string requestUri = string.Format(ConfigurationManager.AppSettings["SearchEmployees"], id);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(requestUri: requestUri).Result;
            response.EnsureSuccessStatusCode();

            // De-serialize the updated product from the response body.
            var json = response.Content.ReadAsStringAsync().Result;

            var users = JsonConvert.DeserializeObject<List<OasisUser>>(json);

            return users?.FirstOrDefault();
        }

        public List<EmpSearch> GetEmpList()
        {
            string json = GetOasisEmployeesJson();

            var users = JsonConvert.DeserializeObject<List<EmpSearch>>(json);

            return users.ToList();
        }

        private static string GetOasisEmployeesJson()
        {
            string fileName = $"EmpList-{DateTime.Now:ddMMyyyyy}.json";

            string targetFolder = HttpContext.Current.Server.MapPath("~/Content");
            string targetPath = $@"{Path.Combine(targetFolder, fileName)}";

            if (File.Exists(targetPath))
            {
                return File.ReadAllText(targetPath);
            }
            else
            {
                string baseUri = ConfigurationManager.AppSettings["HrApiUri"];
                string requestUri = ConfigurationManager.AppSettings["SearchAppraisal"];

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(requestUri: requestUri).Result;
                response.EnsureSuccessStatusCode();

                // De-serialize the updated product from the response body.
                var json = response.Content.ReadAsStringAsync().Result;

                // Write the text to a new file named "Response.json".
                File.WriteAllText(targetPath, json);

                return json;
            }
        }

        #endregion
    }
}
