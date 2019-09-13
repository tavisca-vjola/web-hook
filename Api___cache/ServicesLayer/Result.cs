using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Cache.ServicesLayer
{
    public class Result
    {
        public object Value;
        public List<string> ErrorMessage;
        public int StatusCodes;
        public Result()
        {
            Value = null;
            ErrorMessage = new List<string>();
            StatusCodes = 200;
        }
    }
}
