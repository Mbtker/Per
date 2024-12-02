using Performance.Management.DML.MainEntities;
using System.Data.Entity;
using System.Linq;

namespace Performance.Management.DAL.Repository
{
    public class ReasonRepository
    {
        readonly Entities db = new Entities();

        public IQueryable<Reason> GetAllReason()
        {
            return db.Reasons;
        }

        public Reason GetReason(int ReasonID)
        {
            Reason _Reason = db.Reasons.Find(ReasonID);
            if (_Reason != null)
            {
                return _Reason;
            }
            else
            {
                return null;
            }
        }

        public int InsertReason(Reason _Reason)
        {
            if (_Reason != null)
            {
                _ = db.Reasons.Add(_Reason);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateReason(Reason _Reason)
        {
            if (_Reason != null)
            {
                db.Entry(_Reason).State = EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteReason(Reason _Reason)
        {
            if (_Reason != null)
            {
                _ = db.Reasons.Remove(_Reason);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteReason(int ReasonID)
        {
            Reason _Reason = db.Reasons.Find(ReasonID);
            if (_Reason != null)
            {
                _ = db.Reasons.Remove(_Reason);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

    }
}
