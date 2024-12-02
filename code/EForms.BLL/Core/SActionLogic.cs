using Performance.Management.DAL.Repository;
using Performance.Management.DML.MainEntities;
using System.Collections.Generic;
using System.Linq;

namespace Performance.Management.BLL.Core
{
    public class SActionLogic
    {
        readonly SActionRepository _SActionRepository = new SActionRepository();

        public List<SAction> GetAllSAction()
        {
            return _SActionRepository.GetAllSAction().ToList();
        }

        public SAction GetSAction(int SActionID)
        {
            return _SActionRepository.GetSAction(SActionID);
        }

        public int InsertSAction(SAction _SAction)
        {
            return _SActionRepository.InsertSAction(_SAction);
        }

        public int UpdateSAction(SAction _SAction)
        {
            return _SActionRepository.UpdateSAction(_SAction);
        }

        public int DeleteSAction(SAction _SAction)
        {
            return _SActionRepository.DeleteSAction(_SAction);
        }

        public int DeleteSAction(int SActionID)
        {
            return _SActionRepository.DeleteSAction(SActionID);
        }
    }
}
