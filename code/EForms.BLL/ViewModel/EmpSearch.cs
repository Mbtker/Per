using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Management.BLL.ViewModel
{
    public class EmpSearch
    {
        [JsonProperty("STAFF_ID")]
        public string STAFF_ID { get; set; }

        [JsonProperty("DOC_NUMBER")]
        public string DOC_NUMBER { get; set; }

        [JsonProperty("STAFF_NAME")]
        public string STAFF_NAME { get; set; }

        [JsonProperty("ARBIC_NAME")]
        public string ARBIC_NAME { get; set; }

        [JsonProperty("CONT_START_DATE")]
        public DateTime CONT_START_DATE { get; set; }

        [JsonProperty("CONT_END_DATE")]
        public DateTime CONT_END_DATE { get; set; }

        [JsonProperty("TERMINATION_DATE")]
        public DateTime? TERMINATION_DATE { get; set; }

        [JsonProperty("LAST_WORKING_DATE")]
        public DateTime? LAST_WORKING_DATE { get; set; }

        [JsonProperty("CONTRACT_STATUS")]
        public string CONTRACT_STATUS { get; set; }

        [JsonProperty("DEPT_NAME")]
        public string DEPT_NAME { get; set; }

        [JsonProperty("WORK_ENTITY")]
        public double WORK_ENTITY { get; set; }

        [JsonProperty("POSITION_DESCRIPTION")]
        public string POSITION_DESCRIPTION { get; set; }

        [JsonProperty("DATE_OF_BIRTH")]
        public DateTime DATE_OF_BIRTH { get; set; }

        [JsonProperty("NATIONALITY_USER_CODE")]
        public string NATIONALITY_USER_CODE { get; set; }

        [JsonProperty("NATIONALITY")]
        public string NATIONALITY { get; set; }

        [JsonProperty("DATE_IN_KINGDOM")]
        public DateTime DATE_IN_KINGDOM { get; set; }

        [JsonProperty("SEX")]
        public string SEX { get; set; }

        [JsonProperty("SERVICES")]
        public string SERVICES { get; set; }

        [JsonProperty("AGE")]
        public string AGE { get; set; }

        [JsonProperty("STAFF_CATEGORY")]
        public double STAFF_CATEGORY { get; set; }

        [JsonProperty("CONTRACT_TYPE")]
        public double CONTRACT_TYPE { get; set; }

        [JsonProperty("NATIONALITY_CODE")]
        public double NATIONALITY_CODE { get; set; }

        [JsonProperty("BANNED_FROM_TRAVELING")]
        public string BANNED_FROM_TRAVELING { get; set; }

        [JsonProperty("SX")]
        public string SX { get; set; }

        [JsonProperty("CONTRACT_TYPE_DESC")]
        public string CONTRACT_TYPE_DESC { get; set; }

        [JsonProperty("RELIGION_USER_CODE")]
        public string RELIGION_USER_CODE { get; set; }

        [JsonProperty("RELIGION")]
        public string RELIGION { get; set; }

        [JsonProperty("MAIN_POSITION")]
        public string MAIN_POSITION { get; set; }

        [JsonProperty("MAIN_POSITION_CODE")]
        public double MAIN_POSITION_CODE { get; set; }

        [JsonProperty("WORK_DEPT_NAME")]
        public string WORK_DEPT_NAME { get; set; }

        [JsonProperty("POST_NUMBER")]
        public double POST_NUMBER { get; set; }

        [JsonProperty("POST_DATE_STARTED")]
        public DateTime POST_DATE_STARTED { get; set; }

        [JsonProperty("POST_DATE_ENDED")]
        public DateTime? POST_DATE_ENDED { get; set; }

        [JsonProperty("STAFF_CONTRACT_NO")]
        public double STAFF_CONTRACT_NO { get; set; }

        [JsonProperty("GRADE_STEP")]
        public string GRADE_STEP { get; set; }

        [JsonProperty("MANAGER_STAFF_ID")]
        public string MANAGER_STAFF_ID { get; set; }

        [JsonProperty("MANAGER_NAME")]
        public string MANAGER_NAME { get; set; }

        [JsonProperty("MANAGER_JOB_CODE")]
        public double? MANAGER_JOB_CODE { get; set; }

        [JsonProperty("MANAGER_JOB_TITLE")]
        public string MANAGER_JOB_TITLE { get; set; }

        [JsonProperty("MANAGER_GRADE")]
        public string MANAGER_GRADE { get; set; }
        public bool IsSupervisor { get; set; }
        public int SupervisorUserId { get; set; }
    }
}
