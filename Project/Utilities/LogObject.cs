using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Utilities
{
    public class Request
    {
        public string Path { set; get; }
        public string QueryString { set; get; }
    }

    public class Response
    {
        public int StatusCode { set; get; }
        public dynamic Body { set; get; }
    }

    public class LogObject
    {
        public Request Request { set; get; }
        public Response Response { set; get; }
    }
}
