using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Management.BLL.ViewModel.Enums
{
    public enum RequestTypeEnum
    {
        Supervisory = 1,
        NonSupervisory = 2,
        NonSupervisoryWithCompetenciesOnly = 3,
        unknown = 4
    }
    public enum GoalTypeEnum
    {
        Goal = 1,
        Competency = 2
    }
}
