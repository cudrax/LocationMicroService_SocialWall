using LocationMicroService_SocialWall.DataAccess;
using LocationMicroService_SocialWall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using URISUtil.DataAccess;

namespace LocationMicroService_SocialWall.Controllers
{
    [RoutePrefix("api")]
    public class LocationController : ApiController
    {
        [Route("Location"), HttpGet]
        public IEnumerable<Location> GetAllLocations([FromUri] ActiveStatusEnum active = ActiveStatusEnum.Active)
        {
            return LocationDB.GetLocations(active);
        }

        [Route("Location/{id}"), HttpGet]
        public Location GetLocation(int id)
        {
            return LocationDB.GetLocation(id);
        }

        [Route("Location"), HttpPost]
        public Location InsertLocation(Location location)
        {
            return LocationDB.InsertLocation(location);
        }

        [Route("Location/{id}"), HttpPut]
        public Location UpdateLocation(Location location, int id)
        {
            return LocationDB.UpdateLocation(location, id);
        }

        [Route("Location/{id}"), HttpDelete]
        public void DeleteLocation(int id)
        {
            LocationDB.DeleteLocation(id);
        }
    }
}