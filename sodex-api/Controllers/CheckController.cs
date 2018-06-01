using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sodex_api.Controllers
{
    public class CheckController : ApiController
    {
        [HttpGet, Route("Status")]
        public String GetStatus()
        {
            return "Ok";
        }
    }
}
