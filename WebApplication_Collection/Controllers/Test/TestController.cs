using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication_collection.Models.Filter;
using WebApplication_Collection.Tests.Model;

namespace WebApplication_Collection.Controllers.Test
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {

        [Route("regex")]
        [RegexAttribute]
        public IHttpActionResult Logout(TestRegxAttributeMode tm)
        {
            
            return Ok();
        }
    }
}
