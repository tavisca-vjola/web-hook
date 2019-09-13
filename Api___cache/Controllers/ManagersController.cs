using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Cache.ServicesLayer;
using Api_Cache.DataAccessLayer;
using Api_Cache.DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IManagers _Managers;
        private readonly IServices _services;
        public ManagersController(IManagers managers, IServices services)
        {
            _Managers = managers;
            this._services = services;
        }
        [HttpGet]
        public Result GetResult()
        {
            var result = _services.GetManagers();
            HttpContext.Response.StatusCode = result.StatusCodes;
            return result;
        }
        [HttpGet("{id}")]
        public Result RetreieveById(int id)
        {
            var result = _services.GetManagersById(id);
            HttpContext.Response.StatusCode = result.StatusCodes;
            return result;
        }
        [HttpPost]
        public Result AddManagers([FromBody]Managers m)
        {
            var result = _services.AddManager(m);
            HttpContext.Response.StatusCode = result.StatusCodes;
            return result;
        }
        [HttpPut]
        public Result Update([FromBody]Managers m)
        {
            var result = _services.Update(m);
            HttpContext.Response.StatusCode = result.StatusCodes;
            return result;
        }
        [HttpDelete]
        public object DeleteAll()
        {
            return _services.GetLoggers();
        }
        [HttpDelete("{id}")]

        public Result DeleteManagers(int id)
        {
            var result = _services.DeleteManagerById(id);
            HttpContext.Response.StatusCode = result.StatusCodes;
            return result;
        }

    }
}