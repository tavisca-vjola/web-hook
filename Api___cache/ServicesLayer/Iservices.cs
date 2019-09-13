using Api_Cache.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Cache.ServicesLayer
{
    public interface IServices
    {
        Result GetManagers();
        Result GetManagersById(int id);
        object GetLoggers();
        Result Validate(Managers manager);

        Result AddManager(Managers manager);
        Result Update(Managers manager);
        Result DeleteManagerById(int id);
    }
}
