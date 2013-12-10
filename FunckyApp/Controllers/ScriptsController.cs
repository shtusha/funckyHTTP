using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Concurrent;
using FunckyApp.Models;

namespace FunckyApp.Controllers
{

    public class ScriptsController : ApiController
    {
        private static ConcurrentDictionary<string, Script> repo = new ConcurrentDictionary<string, Script>();

        static ScriptsController()
        { 
            var script = new Script
            {
                Id = "123456789",
                Name = "Test",
            };
            repo[script.Id] = script;
        }

        // GET api/scripts
        public IEnumerable<string> Get()
        {
            return repo.Select(a=> string.Format("{0} {1}", a.Value.Id, a.Value.Name));
        }

        // GET api/scripts/5
        public Script Get(string id)
        {
            Script script;
            if(repo.TryGetValue(id, out script))
            {
                return script;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST api/scripts
        public Script Post(Script value)
        {
            if (value == null) { throw new HttpResponseException(HttpStatusCode.BadRequest); }
            value.Id = Guid.NewGuid().ToString();
            if(!repo.TryAdd(value.Id, value)) { throw new HttpResponseException(HttpStatusCode.Conflict); }

            return value;
        }

        // PUT api/scripts/5
        public void Put(string id, Script value)
        {
            repo[id] = value;
        }

        // DELETE api/scripts/5
        public void Delete(string id)
        {
            Script script;
            if (!repo.TryRemove(id, out script)) { throw new HttpResponseException(HttpStatusCode.NotFound); }
        }
    }
}
