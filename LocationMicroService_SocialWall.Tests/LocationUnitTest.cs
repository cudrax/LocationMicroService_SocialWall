using System;
using URISUtil.DataAccess;
using NUnit.Framework;
using LocationMicroService_SocialWall.Models;
using LocationMicroService_SocialWall.DataAccess;
using System.Text;
using System.Collections.Generic;

namespace LocationMicroService_SocialWall.Tests
{
    public class LocationUnitTest
    {
        ActiveStatusEnum active = ActiveStatusEnum.Active;

        [Test]
        public void GetLocationsSuccess()
        {
            List<Location> locations = LocationDB.GetLocations(active);
            Assert.AreEqual(4, locations.Count);
        }

        [Test]
        public void GetLocationSuccess()
        {
            int id = LocationDB.GetLocations(active)[0].Id;
            Location location = LocationDB.GetLocation(id);
            Assert.IsNotNull(location);
        }

        [Test]
        public void GetLocationFailed()
        {
            int id = 100;
            Location location = LocationDB.GetLocation(id);
            Assert.IsNull(location);

            //Assert.IsNull(LocationDB.GetLocation(id));

        }

        [Test]
        public void InsertLocationSuccess()
        {
            Location location = new Location
            {
                Longitude = 32.43543m,
                Latitude = 14.32456m,
                Active = true
            };
            int oldNumberOfLocations = LocationDB.GetLocations(active).Count;
            LocationDB.InsertLocation(location);
            Assert.AreEqual(oldNumberOfLocations + 1, LocationDB.GetLocations(active).Count);
        }

        [Test]
        public void InsertLocationFailed()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void UpdateLocationSuccess()
        {
            int id = LocationDB.GetLocations(active)[0].Id;
            Location location = new Location
            {
                Longitude = 32.43543m,
                Latitude = 14.32456m,
                Active = true
            };
            Location localLocation = LocationDB.GetLocation(id);
            location.Longitude = localLocation.Longitude;
            location.Latitude = localLocation.Latitude;

            LocationDB.UpdateLocation(location, id);
            Location updatedLocation = LocationDB.GetLocation(id);
            Assert.AreEqual(updatedLocation.Longitude, location.Longitude);


        }

        [Test]
        public void UpdateLocationFailed()
        {
            int id = 100;
            Location location = new Location
            {
                Longitude = 32.43543m,
                Latitude = 14.32456m,
                Active = true
            };
            Location updatedLocation = LocationDB.UpdateLocation(location, id);
            Assert.IsNull(updatedLocation);
        }

        [Test]
        public void DeleteLocationSuccess()
        {
            int id = LocationDB.GetLocations(active)[0].Id;
            LocationDB.DeleteLocation(id);
            Assert.AreEqual(LocationDB.GetLocation(id).Active, false);
        }

        [Test]
        public void DeleteLocationFailed()
        {
            int numberOfOldLocations = LocationDB.GetLocations(active).Count;
            LocationDB.DeleteLocation(100);
            Assert.AreEqual(numberOfOldLocations, LocationDB.GetLocations(active).Count);
        }
    }
}
