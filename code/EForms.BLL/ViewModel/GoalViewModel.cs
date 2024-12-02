using Performance.Management.BLL.ViewModel.Enums;
using Performance.Management.DML.MainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Management.BLL.ViewModel
{
    public class GoalViewModel
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public int RequestID { get; set; }
        public string Name { get; set; }
        public string MeasurementStandard { get; set; }
        public int RelativeWeight { get; set; }
        public string TargetOutput { get; set; }
        public int Fulfillment { get; set; }
    }

    public class CompetencyViewModel
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public int RequestID { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string MeasurementStandard { get; set; }
        public int RelativeWeight { get; set; }
        public string TargetOutput { get; set; }
        public int Fulfillment { get; set; }
        public int CompetencyLookupID { get; set; }

        public List<CompetencyDetails> Details { get; set; }
    }

    public class CompetencyDetails
    {
        public int Index { get; set; }
        public string CompetencyThemes { get; set; }
        public string CompetencyDesc { get; set; }

    }
}
