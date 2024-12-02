namespace Performance.Management.BLL.ViewModel
{
    public enum RequestStatusEnum
    {
        /// <summary>
        /// First Step
        /// </summary>
        RecievedByDepartmentHead = 1,
        SeenByDepartmentHead = 2,
        ApprovedByDepartmentHead = 3,
        RejectedByDepartmentHead = 4,

        /// <summary>
        /// Start - Second Step For Employee
        /// </summary>
        RecievedByEmployee = 5,
        SeenByEmployee = 6,
        ApprovedByEmployee = 7,
        UnderProcessingByEmployee = 9,
        /// <summary>
        /// End - Second Step For Employee
        /// </summary>

        /// <summary>
        /// Last Step HR
        /// </summary>
        RecievedByHRHead = 10,
        ApprovedByHRHead = 11,
        RejectedByHRHead = 12,
        UnderProcessingByHRHead = 13,

        Finished = 14,
        Deleted = 15,

        /// <summary>
        /// **After Rejected from Employee** - DirectManager
        /// </summary>
        RecievedByDirectManager = 16,
        SeenByDirectManager = 17,
        ApprovedByDirectManager = 18,
        RejectedByDirectManager = 19,
        UnderProcessingByDirectManager = 20,


        /// <summary>
        /// **After Rejected from Employee** - DirectManager
        /// </summary>
        RecievedByHRPerformanceUnit = 21,
        SeenByHRPerformanceUnit = 22,
        ApprovedByHRPerformanceUnit = 23,
        UnderProcessingByHRPerformanceUnit = 25,

        RecievedByHRManagerOfPersonnel = 26,
        SeenByHRManagerOfPersonnel = 27,
        ApprovedByHRManagerOfPersonnel = 28,
        RejectedByHRManagerOfPersonnel = 29,
        UnderProcessingByHRManagerOfPersonnel = 30,
    }

}
