using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Cache.DataAccessLayer.Model;

namespace Api_Cache.DataAccessLayer
{
    public class ManagerRepo : IManagers
    {
        public List<Managers> _managers;
        public ManagerRepo()
        {
            _managers = new List<Managers>();
        }
        public bool AddManager(Managers managers)
        {
            var _employee = _managers.Find(X => X.Mid == managers.Mid);
            if (_employee == null)
            {
                _managers.Add(managers);
                return true;
            }
            else
                return false;
        }

        public bool DeleteManager(int id)
        {
            var _book = _managers.Find(X => X.Mid == id);
            if (_book == null)
            {
                return false;
            }
            else
            {
                var index = _managers.IndexOf(_book);
                _managers.RemoveAt(index);

                return true;
            }
        }

        public List<Managers> GetAllManagers()
        {
                return _managers;
        }

        public Managers GetById(int id)
        {
            var _m = _managers.Find(X => X.Mid == id);
            if (_m == null)
                return null;
            return _m;

        }

        public bool ReplaceManager(Managers managers)
        {
            var _m = _managers.Find(X => X.Mid == managers.Mid);
            if (_m == null)
            {
                return false;
            }
            else
            {
                var index = _managers.IndexOf(_m);
                _managers[index] = _m;

                return true;
            }
        }
    }
}
