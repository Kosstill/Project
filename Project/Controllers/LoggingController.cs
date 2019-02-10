using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Project.Utilities;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly string _path;

        public LoggingController()
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "logging.txt");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LogObject>> Get()
        {
            string[] objectLines = System.IO.File.ReadAllLines(_path);

            var objects = new List<LogObject>();
            foreach ( var objectLine in objectLines )
            {
                objects.Add(JsonConvert.DeserializeObject<LogObject>(objectLine));
            }

            return objects;
        }
    }
}