using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace What_The_Hike
{
    public class PointOfInterestView
    {
        public int poiID { get; set; }

        public String PointOfInterestDescription { get; set; }

    }

    public class FinalFacility
    {
        public string name { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        public bool pets { get; set; }

        public bool parking { get; set; }

        public bool bookingRequired { get; set; }

        public string MondayOpHours { get; set; }

        public string TuesdayOpHours { get; set; }

        public string WednesdayOpHours { get; set; }

        public string ThursdayOpHours { get; set; }

        public string FridayOpHours { get; set; }

        public string SaturdayOpHours { get; set; }

        public string SundayOpHours { get; set; }


    }

}
