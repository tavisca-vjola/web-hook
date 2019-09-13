using Api_Cache.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Cache.DataAccessLayer
{
    public interface IManagers
    {
        bool AddManager(Managers managers);
        bool ReplaceManager(Managers managers);
        Managers GetById(int id);
        List<Managers> GetAllManagers();
        bool DeleteManager(int id);
    }
}
