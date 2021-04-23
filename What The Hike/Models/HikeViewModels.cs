using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace What_The_Hike
{
    public class ReturnObject
    {
        public bool success { get; set; }
        public String message { get; set; }
        public Object data { get; set; }
    }

    public class PointOfInterestView
    {
        public int poiID { get; set; }

        public String PointOfInterestDescription { get; set; }

    }

    public class HikeSimpleView
    {
        public int hikeID { get; set; }
        public String name { get; set; }
    }

    public class HikeGeneralView
    {
        public int hikeID { get; set; }

        public String name { get; set; }

        public byte[] map { get; set; }

        public String description { get; set; }

        public String coordinates { get; set; }

        public float distance { get; set; }

        public float enteranceFee { get; set; }

        public short maxGroupSize { get; set; }

        public String difficulty { get; set; }

        public float duration { get; set; }

        public int facilityId { get; set; }

        public String facilityName { get; set; }
    }

    public class HikeDetailView
    {
        public int hikeID { get; set; }

        public String name { get; set; }

        public byte[] map { get; set; }

        public String description { get; set; }

        public String coordinates { get; set; }

        public float distance { get; set; }

        public float enteranceFee { get; set; }

        public short maxGroupSize { get; set; }

        public String difficulty { get; set; }

        public float duration { get; set; }

        public FacilityView facility { get; set; }

        public List<PointOfInterestView> pointsOfInterest { get; set; }

    }

    public class FacilityView
    {
        public int facilityID { get; set; }

        public String name { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        public bool parking { get; set; }

        public bool pets { get; set; }

        public bool bookingRequired { get; set; }

        //public virtual ICollection<FacilityHoursLink> FacilityHoursLink { get; set; }

        public List<HikeSimpleView> HikesInFacility { get; set; }
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

    public class UserHikeScore
    {
        public int User { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double TotalHikingDistance { get; set; }
    }

    public class HikeFromLog
    {
        public string name { get; set; }

        public string description { get; set; }

        public string difficulty { get; set; }

        public string distance { get; set; }

        public string facility { get; set; }

        public string duration { get; set; }
    }

    public class UserHikeLog
    {
        public User user { get; set; }

        public HikeLog hikeLog { get; set; }
    }
}
