using Performance.Management.DAL.Repository;
using Performance.Management.DML.MainEntities;
using System.Collections.Generic;
using System.Linq;

namespace Performance.Management.BLL.Core
{
    public class StatusLogic
    {
        readonly StatusRepository _StatusRepository = new StatusRepository();

        public List<Status> GetAllStatus()
        {
            return _StatusRepository.GetAllStatus().ToList();
        }

        public IQueryable<Status> GetAll()
        {
            return _StatusRepository.GetAllStatus();
        }

        public Status GetStatus(int StatusID)
        {
            return _StatusRepository.GetStatus(StatusID);
        }

        public int InsertStatus(Status _Status)
        {
            return _StatusRepository.InsertStatus(_Status);
        }

        public int UpdateStatus(Status _Status)
        {
            return _StatusRepository.UpdateStatus(_Status);
        }

        public int DeleteStatus(Status _Status)
        {
            return _StatusRepository.DeleteStatus(_Status);
        }

        public int DeleteStatus(int StatusID)
        {
            return _StatusRepository.DeleteStatus(StatusID);
        }
    }
}
