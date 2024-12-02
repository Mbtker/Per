using Performance.Management.DML.MainEntities;
using System.Data.Entity;
using System.Linq;

namespace Performance.Management.DAL.Repository
{
    public class SActionRepository
    {
        readonly Entities db = new Entities();

        public IQueryable<SAction> GetAllSAction()
        {
            return db.SActions;
        }

        public SAction GetSAction(int SActionID)
        {
            SAction _SAction = db.SActions.Find(SActionID);
            if (_SAction != null)
            {
                return _SAction;
            }
            else
            {
                return null;
            }
        }

        public int InsertSAction(SAction _SAction)
        {
            if (_SAction != null)
            {
                _ = db.SActions.Add(_SAction);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateSAction(SAction _SAction)
        {
            if (_SAction != null)
            {
                db.Entry(_SAction).State = EntityState.Modified;
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteSAction(SAction _SAction)
        {
            if (_SAction != null)
            {
                _ = db.SActions.Remove(_SAction);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteSAction(int SActionID)
        {
            SAction _SAction = db.SActions.Find(SActionID);
            if (_SAction != null)
            {
                _ = db.SActions.Remove(_SAction);
                return db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

    }
}
