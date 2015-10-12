using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DataModels;

namespace StorePdfToSQLThenReadOut.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values
        [HttpGet]
        public HttpResponseMessage PdfGet(int id)
        {
            using (var db = new SignedContractContext())
            {
                var entry = db.SignedContracts.Find(id);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(new MemoryStream(entry.SignedFile));
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/pdf");
                return result;
            }
        }
    }
}
