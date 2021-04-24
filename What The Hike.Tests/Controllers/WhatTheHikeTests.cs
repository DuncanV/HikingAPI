using System;
using NUnit.Framework;
using What_The_Hike;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace What_The_Hike_Testing.Controllers
{
    [TestFixture]
    public class WhatTheHikeTests
    {
        AdminController AdminController = new AdminController();
        HikeController HikeController = new HikeController();

        /* Start of Admin tests */
        [Test]
        public void TestLocationDetails()
        {
            var results = AdminController.GetLocation(1);
            var dataType = results.Data.GetType();
            Assert.AreEqual("Moreleta Kloof Nature Area", dataType.GetProperty("name").GetValue(results.Data));
            Assert.IsFalse((bool?)dataType.GetProperty("pets").GetValue(results.Data));
            Assert.AreNotEqual(false, dataType.GetProperty("parking").GetValue(results.Data));
            Assert.False((bool)dataType.GetProperty("bookingRequired").GetValue(results.Data));
            Assert.IsNotEmpty((string)dataType.GetProperty("SundayOpHours").GetValue(results.Data));
            Assert.AreNotEqual(dataType.GetProperty("latitude").GetValue(results.Data), dataType.GetProperty("longitude").GetValue(results.Data));
        }

        [Test]
        public void TestLoadOperatingHours()
        {
            var results = AdminController.LoadOperatingHours(1, 10, 2);
            var json = (JObject)JsonConvert.DeserializeObject(results);
            Assert.AreEqual("Operating hours already exists", json["message"].Value<String>());
            Assert.IsFalse(json["Success"].Value<bool>());
        }

        [Test]
        public void TestLoadFacilityHoursDuplicate()
        {
            var results = AdminController.LoadFacilityHours(1, 1);
            var json = (JObject)JsonConvert.DeserializeObject(results);
            Assert.AreEqual("Facility Hours already exists", json["message"].Value<String>());
            Assert.IsFalse(json["Success"].Value<bool>());
        }

        [Test]
        public void TestLoadFacilityUnavailableHours()
        {
            var results = AdminController.LoadFacilityHours(1, 123);
            var json = (JObject)JsonConvert.DeserializeObject(results);
            Assert.AreEqual("Could not add FacilityHoursLink because Operating Hours id does not exist", json["message"].Value<String>());
            Assert.IsFalse(json["Success"].Value<bool>());
        }

        [Test]
        public void TestLoadLocation()
        {
            var results = AdminController.LoadLocation("Kaladiwele Mountains", (float)-25.9876, (float)18.3546, false, false, true);
            var json = (JObject)JsonConvert.DeserializeObject(results);
            Assert.AreNotEqual("Facility already exists", json["message"].Value<String>());
            Assert.IsTrue(json["Success"].Value<bool>());
        }
        /* End of Admin tests */


        /* Start of Hike tests */
        [Test]
        public void TestGetHike()
        {
            var results = HikeController.GetHike(1);
            var dataType = results.Data.GetType();
            var test = dataType.GetProperty("map").GetValue(results.Data);
            Assert.IsNull(test);
        }

        [Test]
        public void TestWeatherAtLocation()
        {
            var results = HikeController.Weather(1);
            var json = (JObject)JsonConvert.DeserializeObject(results);
            Assert.Greater(json["daily"][2]["sunset"].Value<int>(), json["daily"][1]["sunrise"].Value<int>());
        }

        [Test]
        public void TestLeaderBoard()
        {
            var results = HikeController.LeaderBoard();
            var json = (JArray)JsonConvert.DeserializeObject(results);
            Assert.LessOrEqual(json[1]["Value"].Value<double>(), json[0]["Value"].Value<double>());
        }
        /* End of Hike tests */
    }
}