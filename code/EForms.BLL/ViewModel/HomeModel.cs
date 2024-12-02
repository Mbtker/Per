using System;

namespace Performance.Management.BLL.ViewModel
{
    public class HomeModel
    {

        public int AllRequestCount { get; set; }
        public int FinishedRequestCount { get; set; }
        public int UnderProcessRequestCount { get; set; }
        public int DelayRequestCount { get; set; }
        public int DepartmentApprovalCount { get; set; }
        public Double DepartmentApprovalRatio { get; set; }
        public int DepartmentDisapprovalCount { get; set; }
        public Double DepartmentDisapprovalRatio { get; set; }
        public int PORApprovalCount { get; set; }
        public Double PORApprovalRatio { get; set; }
        public int PRDisapprovalCount { get; set; }
        public Double PRDisapprovalRatio { get; set; }
        public int AchievementFeedback25Count { get; set; }
        public int AchievementFeedback50Count { get; set; }
        public int AchievementFeedback75Count { get; set; }
        public int AchievementFeedback100Count { get; set; }
        public int TimeOfServiceFeedback25Count { get; set; }
        public int TimeOfServiceFeedback50Count { get; set; }
        public int TimeOfServiceFeedback75Count { get; set; }
        public int TimeOfServiceFeedback100Count { get; set; }
        public int SatisfacationFeedback25Count { get; set; }
        public int SatisfacationFeedback50Count { get; set; }
        public int SatisfacationFeedback75Count { get; set; }
        public int SatisfacationFeedback100Count { get; set; }
        public int FormAdminFinishedCount { get; set; }
        public Double FormAdminFinishedRatio { get; set; }
        public int FormAdminUnderProcessingCount { get; set; }
        public Double FormAdminUnderProcessingRatio { get; set; }
        public int FormAdminDelaydCount { get; set; }
        public Double FormAdminDelaydRatio { get; set; }
        public string Elapsed { get; set; }
        public int EmpCount { get; set; }
        public int EmpSupervisoryCount { get; set; }
        public int EmpNonSupervisoryCount { get; set; }
        public double EmpCountRatio { get; set; }
        public double EmpSupervisoryCountRatio { get; set; }
        public double EmpNonSupervisoryCountRatio { get; set; }
        public int EmpNonSupervisoryCompetenciesOnlyCount { get; set; }
        public int EmpNonUnknownCount { get; set; }
        public int EmpAllNonSupervisoryCount { get; set; }
        public int JobCountTotal { get; set; }
        public int JobCountForBusy { get; set; }
        public int JobCountForVacancy { get; set; }
        public double JobCountTotalRaion { get; set; }
        public double JobCountForBusyRatio { get; set; }
        public double JobCountForVacancyRatio { get; set; }
    }
}
