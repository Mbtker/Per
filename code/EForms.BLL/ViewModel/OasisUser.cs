using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Performance.Management.BLL.ViewModel
{
    public class OasisUser
    {
        [JsonProperty("doc_number")]
        public string DocNumber { get; set; }

        [JsonProperty("staff_id")]
        public string StaffId { get; set; }

        [JsonProperty("staff_name_ar")]
        public string StaffNameAr { get; set; }

        [JsonProperty("staff_name_en")]
        public string StaffNameEn { get; set; }

        [JsonProperty("hiring_date_ar")]
        public string HiringDateAr { get; set; }

        [JsonProperty("nat_ar")]
        public string NatAr { get; set; }

        [JsonProperty("hiring_date_en")]
        public string HiringDateEn { get; set; }

        [JsonProperty("nat_en")]
        public string NatEn { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("cont_start_date_en")]
        public string ContStartDateEn { get; set; }

        [JsonProperty("cont_start_date_ar")]
        public string ContStartDateAr { get; set; }

        [JsonProperty("cont_end_date_en")]
        public string ContEndDateEn { get; set; }

        [JsonProperty("cont_end_date_ar")]
        public string ContEndDateAr { get; set; }

        [JsonProperty("today_en")]
        public string TodayEn { get; set; }

        [JsonProperty("today_ar")]
        public string TodayAr { get; set; }

        [JsonProperty("position_ar")]
        public string PositionAr { get; set; }

        [JsonProperty("position_en")]
        public string PositionEn { get; set; }

        [JsonProperty("department_en")]
        public string DepartmentEn { get; set; }

        [JsonProperty("department_ar")]
        public string DepartmentAr { get; set; }

        [JsonProperty("patient_no")]
        public string PatientNo { get; set; }

        [JsonProperty("contract_type_en")]
        public string ContractTypeEn { get; set; }

        [JsonProperty("contract_type_ar")]
        public string ContractTypeAr { get; set; }

        [JsonProperty("contract_status_en")]
        public string ContractStatusEn { get; set; }

        [JsonProperty("contract_status_ar")]
        public string ContractStatusAr { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("work_entity")]
        public string WorkEntity { get; set; }

        [JsonProperty("position_type")]
        public string PositionType { get; set; }

        [JsonProperty("report_to_name")]
        public string ReportToName { get; set; }

        [JsonProperty("report_to_nid")]
        public string ReportToNationalId { get; set; }

        [JsonProperty("report_to_id")]
        public string ReportToId { get; set; }

        [JsonProperty("report_to_email")]
        public string ReportToEmail { get; set; }

        [JsonProperty("report_to_dept")]
        public string ReportToDept { get; set; }

        [JsonProperty("report_to_job_title")]
        public string ReportToJobTitle { get; set; }

        [JsonProperty("report_to_post_number")]
        public string ReportToPostNumber { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; internal set; }
    }
}
