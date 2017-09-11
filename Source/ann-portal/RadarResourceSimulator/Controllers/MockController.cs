using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RadarResourceSimulator.Controllers
{
    public class MockController : ApiController
    {
        [HttpPost]
        public object GetUserDetails(GetUserDetailsRequest request)
        {
            return new { userId = request.Token, firstName = "Bla Bla", lastName = "Bla Bla" };
        }
    }

    public class GetUserDetailsRequest
    {
        public string Token { get; set; }
    }
}
