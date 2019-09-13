using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Cache.ServicesLayer
{
    public class Loggers
    {
        public string Time { set; get; }
        public string Event { set; get; }
        public int StatusCode { set; get; }
        public List<string> Error { set; get; }
    } 
}
