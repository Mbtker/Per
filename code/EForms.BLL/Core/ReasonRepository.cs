using Performance.Management.DAL.Repository;
using Performance.Management.DML.MainEntities;
using System.Linq;

namespace Performance.Management.BLL.Core
{
    public class ReasonLogic
    {
        readonly ReasonRepository _ReasonRepository = new ReasonRepository();

        public IQueryable<Reason> GetAllReason()
        {
            return _ReasonRepository.GetAllReason();
        }

        public Reason GetReason(int ReasonID)
        {
            return _ReasonRepository.GetReason(ReasonID);
        }

        public int InsertReason(Reason _Reason)
        {
            return _ReasonRepository.InsertReason(_Reason);
        }

        public int UpdateReason(Reason _Reason)
        {
            return _ReasonRepository.UpdateReason(_Reason);
        }

        public int DeleteReason(Reason _Reason)
        {
            return _ReasonRepository.DeleteReason(_Reason);
        }

        public int DeleteReason(int ReasonID)
        {
            return _ReasonRepository.DeleteReason(ReasonID);
        }
    }
}
