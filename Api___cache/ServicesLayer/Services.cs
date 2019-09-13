using Api_Cache.DataAccessLayer;
using Api_Cache.DataAccessLayer.Model;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Cache.ServicesLayer
{
    public class Services:IServices
    {
        private readonly IManagers _managers;
        private readonly ManagerValidationThroughFluent _managervalidthroughfluent;
        private readonly List<Loggers> _loggers;
        private readonly RedisManagerPool Pool = new RedisManagerPool("localhost:6379") ;
        IRedisClient Client;
        public Services(IManagers managers)
        {
            _managers = managers;
            _managervalidthroughfluent = new ManagerValidationThroughFluent();
            _loggers = new List<Loggers>();
            Client = Pool.GetClient();
            Client.FlushAll();
        }

        public Result AddManager(Managers manager)
        {
            Result result;
            result = Validate(manager);
            if(result.ErrorMessage.Count!=0)
            {
                Loggers logger = new Loggers
                {
                    Error = result.ErrorMessage,
                    Time = DateTime.Now.ToString(),
                    Event= $"AddingManager{manager}",
                    StatusCode= result.StatusCodes
                };
                _loggers.Add(logger);
                return result;
            }
            var status = _managers.AddManager(manager);
            if(status== true)
            {
                result.Value = "Success";
            }
            else
            {
                result.ErrorMessage.Add("Already Exists");
                result.StatusCodes = 404;
            }
            Loggers ulogger = new Loggers
            {
                Error = result.ErrorMessage,
                Time = DateTime.Now.ToString(),
                Event = $"AddManager",
                StatusCode = result.StatusCodes

            };
            _loggers.Add(ulogger);
            return result;
        }

        public Result DeleteManagerById(int id)
        {
            Result result = new Result();
            if (id < 0)
            {
                result.ErrorMessage.Add("Invalid id  Found");
                result.StatusCodes = 400;
                result.Value = null;
                Loggers logger = new Loggers
                {
                    Error = result.ErrorMessage,
                    Time = DateTime.Now.ToString(),
                    Event = $"GetManagerById{id}",
                    StatusCode = result.StatusCodes,
                };
                _loggers.Add(logger);
                return result;

            }

            var manager = _managers.DeleteManager(id);
            if(manager==false)
            {
                result.ErrorMessage.Add("Error deleting manager");
                result.StatusCodes = 404;
            }
            else
            {
                Client.Remove(id.ToString());
                result.Value = "Deleted";

            }
            Loggers logger1 = new Loggers
            {
                Error = result.ErrorMessage,
                Time = DateTime.Now.ToString(),
                Event = $"DeleteELementbyid{id}",
                StatusCode = result.StatusCodes

            };
            _loggers.Add(logger1);

            return result;
        }

        public object GetLoggers()
        {
            FileStream file = new FileStream("LogFile.txt",FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            foreach(var logger in _loggers)
            {
                sw.WriteLine("TIME :     "+logger.Time);
                if(logger.Error.Count>0)
                    sw.WriteLine("Errors: ");
                var logvalue = "";
                foreach(var error in logger.Error)
                {
                    logvalue += error;
                    logvalue += ",    ";

                }
                if (logvalue.Length > 0)
                    sw.WriteLine(logvalue);
                sw.WriteLine("Event    "+logger.Event);
                sw.WriteLine("StatusCode    "+logger.StatusCode.ToString());
                logvalue = "\t---------------------------------------------------------\n";
                    sw.WriteLine(logvalue);
                
            }
            sw.Flush();
            sw.Close();
            file.Close();
            return _loggers;
        }

        public Result GetManagers()
        {
            var managers = _managers.GetAllManagers();
            Result result = new Result();
            if(managers.Count==0)
            {
                result.ErrorMessage.Add("No Manager Found");
                result.StatusCodes = 404;
                result.Value = null;
                Loggers logger = new Loggers { Error = result.ErrorMessage,Time=DateTime.Now.ToString(),Event="GetManagers",
                    StatusCode=result.StatusCodes,};
                _loggers.Add(logger);
              
            }
            else
            {
                
                result.StatusCodes = 200;
                result.Value = managers;
                Loggers logger = new Loggers
                {
                    Error = result.ErrorMessage,
                    Time = DateTime.Now.ToString(),
                    Event = "GetManagers",
                    StatusCode = result.StatusCodes,
                };
                _loggers.Add(logger);

            }


            return result;

        }

        public Result GetManagersById(int id)
        {
          
            Result result = new Result();
            if (id<0)
            {
                result.ErrorMessage.Add("Invalid id  Found");
                result.StatusCodes = 400;
                result.Value = null;
                Loggers logger = new Loggers
                {
                    Error = result.ErrorMessage,
                    Time = DateTime.Now.ToString(),
                    Event = $"GetManagerById{id}",
                    StatusCode = result.StatusCodes,
                };
                _loggers.Add(logger);
                return result;

            }
            Loggers logger1;
            Managers m;
            if(Client.Get<Managers>(id.ToString())!=null)
            {
                m = Client.Get<Managers>(id.ToString());
                logger1 = new Loggers
                {
                    Error = result.ErrorMessage,
                    Time = DateTime.Now.ToString(),
                    Event = $"retrieved from cache",
                    StatusCode = result.StatusCodes,
                };
                _loggers.Add(logger1);
            }
            else
            {
                m = _managers.GetById(id);
                if(m==null)
                {
                    result.ErrorMessage.Add("Invalid Id ");
                    result.StatusCodes = 404 ;

                }
                else
                {
                    Client.Set(m.Mid.ToString(),m);
                    logger1 = new Loggers
                    {
                        Error = result.ErrorMessage,
                        Time = DateTime.Now.ToString(),
                        Event = $"Added in cache",
                        StatusCode = result.StatusCodes,
                    };
                    _loggers.Add(logger1);
                }
            }
            result.Value = m;
            logger1 = new Loggers
            {
                Error = result.ErrorMessage,
                Time = DateTime.Now.ToString(),
                Event = $"GetBookByid{id}",
                StatusCode = result.StatusCodes,
            };
            _loggers.Add(logger1);


            return result;
        }

        public Result Update(Managers manager)
        {
            Result result;
            result = Validate(manager);
            if(result.ErrorMessage.Count!=0)
            {
                return result;
            }
            if (_managers.ReplaceManager(manager))
            {
                Client.Remove(manager.Mid.ToString());
                Client.Set(manager.Mid.ToString(),manager);
                result.Value = "Success";
            }
            else
            {
                result.ErrorMessage.Add("No Id Match");
                result.StatusCodes = 404;
            }
            Loggers Ulogger = new Loggers
            {
                Error = result.ErrorMessage,
                Time=DateTime.Now.ToString(),
                Event=$"UpdateManager",
                StatusCode=result.StatusCodes
            };
            _loggers.Add(Ulogger);
            return result;
        }

        public Result Validate(Managers manager)
        {
            Result result = new Result();
            var resultfluent = _managervalidthroughfluent.Validate(manager);
            if(!resultfluent.IsValid)
            {
                foreach(var failure in resultfluent.Errors)
                {
                    result.ErrorMessage.Add(failure.ErrorMessage);
                }
                result.StatusCodes = 400;
            }
            return result;
            
        }
    }
}
