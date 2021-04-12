using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using System.Net.Http;

namespace What_The_Hike.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        
        public String GetAllHikesLogged()
        { 
            string version = string.Empty;
            using (MySqlConnection dbConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["mySQLConnection"].ConnectionString))
            {
                MySqlDataReader reader;
                using (MySqlCommand cmd = new MySqlCommand("SELECT h.name, CONCAT(u.name,' ', u.surname) as 'User Name' FROM `hikelog` hl LEFT OUTER JOIN user u ON hl.userID = u.userID LEFT OUTER JOIN hike h ON hl.hikeID = h.hikeID", dbConn))
                {
                    try
                    {
                        dbConn.Open();
                        // If the db cannot be connected to the reader won't be created -> No need to try close in Catch
                        reader = cmd.ExecuteReader();
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        version = JsonConvert.SerializeObject(dataTable);
                    }
                    catch (Exception erro)
                    {
                        ViewBag.Add("Erro - " + erro.Message);
                        dbConn.Close();
                    }
                }
            }

            return version;
        }


        [HttpGet]
        public String getTempWeather()
        {
            string Ret = string.Empty;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/");
            var latitude = -25.8321;
            var longitiude = 28.2914;
            var apiKey = "7db484b6881763be58be890766009069";
            string endpoint = $"data/2.5/onecall?lat={latitude}&lon={longitiude}&units=metric&appid={apiKey}";

            HttpResponseMessage response = client.GetAsync(endpoint).Result;
            if (response.IsSuccessStatusCode)
            {
                Ret = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Ret = "{sucess: false, message: \"WeatherApi call failed\"}";
            }
            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return Ret;
        }
    }
}