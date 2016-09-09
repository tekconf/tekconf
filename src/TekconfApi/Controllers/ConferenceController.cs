using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TekconfApi.DataObjects;
using TekconfApi.Models;

namespace TekconfApi.Controllers
{
    public class ConferenceController : TableController<Conference>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Conference>(context, Request);
        }

        // GET tables/Conference
        public IQueryable<Conference> GetAllConferences()
        {
            return Query();
        }

        // GET tables/Conference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Conference> GetConference(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Conference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Conference> PatchConference(string id, Delta<Conference> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Conference
        public async Task<IHttpActionResult> PostConference(Conference item)
        {
            Conference current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Conference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteConference(string id)
        {
            return DeleteAsync(id);
        }
    }
}