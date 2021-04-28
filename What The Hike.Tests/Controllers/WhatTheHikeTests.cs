using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace What_The_Hike.Controllers.Tests
{
    [TestClass()]
    public class WhatTheHikeTests
    {

        AdminController AdminController = new AdminController();
        HikeController HikeController = new HikeController();

        [TestMethod()]
        public void TestLocationDetails()
        {
            var results = AdminController.GetLocation(1);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreEqual("Moreleta Kloof Nature Area", json["data"]["name"].Value<String>());
            Assert.IsFalse(json["data"]["pets"].Value<bool>());
            Assert.AreNotEqual(false, json["data"]["parking"].Value<bool>());
            Assert.IsFalse(json["data"]["bookingRequired"].Value<bool>());
            Assert.AreNotEqual(json["data"]["SundayOpHours"].Value<string>(), "");
            Assert.AreNotEqual(json["data"]["latitude"].Value<string>(), json["data"]["longitude"].Value<string>());
        }

        [TestMethod()]
        public void TestLoadOperatingHours()
        {
            var results = AdminController.LoadOperatingHours(1, 10, 2);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreEqual("Operating Hours already exists", json["message"].Value<String>());
            Assert.IsFalse(json["success"].Value<bool>());
        }

        [TestMethod()]
        public void TestLoadFacilityHoursDuplicate()
        {
            var results = AdminController.LoadFacilityHours(1, 1);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreEqual("Facility Hours already exists", json["message"].Value<String>());
            Assert.IsFalse(json["success"].Value<bool>());
        }

        [TestMethod()]
        public void TestLoadFacilityUnavailableHours()
        {
            var results = AdminController.LoadFacilityHours(1, 123);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreEqual("Could not add FacilityHoursLink because Operating Hours id does not exist", json["message"].Value<String>());
            Assert.IsFalse(json["success"].Value<bool>());
        }

        [TestMethod()]
        public void TestLoadLocation()
        {
            var results = AdminController.LoadLocation("Kaladiwele Mountains", (float)-25.9876, (float)18.3546, false, false, true);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreEqual("Facility already exists", json["message"].Value<String>());
            Assert.IsFalse(json["success"].Value<bool>());
        }
        /* End of Admin tests */


        /* Start of Hike tests */
        [TestMethod()]
        public void TestGetHike()
        {
            var results = HikeController.GetHike(1);
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.IsNull(json.GetValue("map"));
        }

        [TestMethod()]
        public void TestWeatherAtLocation()
        {
            var results = HikeController.Weather(1);
            var json = JObject.Parse(results.Content);
            Assert.AreNotEqual(json["data"]["daily"][2]["sunset"].Value<int>(), json["data"]["daily"][1]["sunrise"].Value<int>());
        }

        [TestMethod()]
        public void TestLeaderBoard()
        {
            var results = HikeController.LeaderBoard();
            var json = JObject.Parse(JsonConvert.SerializeObject(results.Data));
            Assert.AreNotEqual(json["data"][1]["TotalHikingDistance"].Value<double>(), json["data"][0]["TotalHikingDistance"].Value<double>());
        }

        [TestMethod()]
        public void CreateTest()
        {
            var results = HikeController.Create(new HikeLog_UserHikeID() { hikeID = "1", userID = "3" });
            var json = JObject.Parse(results.Content);
            Assert.AreEqual("Created New Log.", json["message"].Value<String>());
            Assert.IsTrue(json["success"].Value<bool>());
        }
    }
}
