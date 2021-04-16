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
}
