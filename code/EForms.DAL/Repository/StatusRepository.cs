using Performance.Management.DML.MainEntities;
using System.Data.Entity;
using System.Linq;

namespace Performance.Management.DAL.Repository
{
    public class StatusRepository
    {
        readonly Entities db = new Entities();

        public IQueryable<Status> GetAllStatus()
        {
            return db.Status;
        }

        public Status GetStatus(int StatusID)
        {
            Status _Status = db.Status.Find(StatusID);
            if (_Status != null)
            {
                return _Status;
            }
            else
            {
                return null;
            }
        }

        public int InsertStatus(Status _Status)
        {
            if (_Status != null)
            {
                _ = db.Status.Add(_Status);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateStatus(Status _Status)
        {
            if (_Status != null)
            {
                db.Entry(_Status).State = EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteStatus(Status _Status)
        {
            if (_Status != null)
            {
                _ = db.Status.Remove(_Status);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteStatus(int StatusID)
        {
            Status _Status = db.Status.Find(StatusID);
            if (_Status != null)
            {
                _ = db.Status.Remove(_Status);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

    }
}
