using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
/**Contains classes that can be used for serializing and deserializing objects. Serialization is the process of converting an object or a graph of objects into a linear sequence of bytes for either storage or transmission to another location. Deserialization is the process of taking in stored information and recreating objects from it.*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using What_The_Hike.Models;

namespace What_The_Hike
{

    public class Day 
    {
        [DataMember]
        [Key]
        public int dayID { get; set; }
        
        [DataMember]
        [Required]
        [MaxLength(32)]
        [DefaultValue(null)]
        public String description { get; set; }
    }
    public class Difficulty
    {
        [DataMember]
        [Key]
        public int difficultyID { get; set; }

        [DataMember]
        [Required]
        [MaxLength(255)]
        [DefaultValue(null)]
        public String description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Hike> Hike { get; set; }
    }
    public class Duration
    {
		[DataMember]
		[Key]
        public int durationID { get; set; }

        [DataMember]
        [DefaultValue(0)]
        public float time { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Hike> Hike { get; set; }
    }
    public class Facility
    {
		[DataMember]
		[Key]
        public int facilityID { get; set; }

        [DataMember]
        [Required]
		[MaxLength(32)]
		public String name { get; set; }

        [DataMember]
        [DefaultValue(null)]
        public float latitude { get; set; }

        [DataMember]
        [DefaultValue(null)]
        public float longitude { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        [DefaultValue(true)]
        public bool parking { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        [DefaultValue(true)]
        public bool pets { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        [DefaultValue(true)]
        public bool bookingRequired { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FacilityHoursLink> FacilityHoursLink { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Hike> Hike { get; set; }
    }
    public class FacilityHoursLink 
    {
        [DataMember]
        [Key]
        public int facilityHoursLinkID { get; set; }


        [DataMember(IsRequired = true)]
        [Required]
        public int facilityID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("facilityID")]
        public virtual Facility Facility { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int operatingHoursID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("operatingHoursID")]
        public virtual OperatingHours OperatingHours { get; set; }
    }
    public class Hike
    {
        [DataMember]
        [Key]
        public int hikeID { get; set; }

        [DataMember]
        [Required]
        [MaxLength(32)]
        public String name { get; set; }

        [DataMember]
        [Required]
        [DefaultValue(null)]
        public byte[] map { get; set; }

        [DataMember]
        [MaxLength(255)]
        [DefaultValue(null)]
        public String description { get; set; }

        [DataMember]
        [DefaultValue(null)]
        public String coordinates { get; set; }

        [DataMember]
        [Required]
        [DefaultValue(0)]
        public float distance { get; set; }

        [DataMember]
        [Required]
        [DefaultValue(0)]
        public float enteranceFee { get; set; }

        [DataMember]
        [Required]
        [DefaultValue(1)]
        public short maxGroupSize { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int difficultyID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("difficultyID")]
        public virtual Difficulty Difficulty { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int avgDuration { get; set; }
        [IgnoreDataMember]
        [ForeignKey("avgDuration")]
        public virtual Duration Duration { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int facilityID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("facilityID")]
        public virtual Facility Facility { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<HikeInterestLink> HikeInterestLink { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<HikeLog> HikeLog { get; set; }
    }
    public class HikeInterestLink
    {
        [DataMember]
        [Key]
        public int hikeInterestID { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int hikeID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("hikeID")]
        public virtual Hike Hike{ get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int pointOfInterestID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("pointOfInterestID")]
        public virtual PointOfInterest PointOfInterest { get; set; }

    }
    public class HikeLog
    {
        [DataMember]
        [Key]
        public int hikeLogID { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int hikeID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("hikeID")]
        public virtual Hike Hike { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int userID { get; set; }
        [IgnoreDataMember]
        [ForeignKey("userID")]
        public virtual User User { get; set; }
    }
    public class OperatingHours 
    {
		[DataMember]
        [Key]
        public int operatingHoursID { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int time_from { get; set; }
        [IgnoreDataMember]
        [ForeignKey("time_from")]
        public virtual Time Time_From { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int time_to { get; set; }
        [IgnoreDataMember]
        [ForeignKey("time_to")]
        public virtual Time Time_To { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int day { get; set; }
        [IgnoreDataMember]
        [ForeignKey("day")]
        public virtual Day Day { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FacilityHoursLink> FacilityHoursLink { get; set; }
    }
    public class PointOfInterest
    {
		[DataMember]
		[Key]
		public int pointOfInterestID { get; set; }

		[DataMember]
		[MaxLength(255)]
        [Display(Name = "Points of Interest Description")]
        [DefaultValue(null)]
        public string description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<HikeInterestLink> HikeInterestLink { get; set; }
    }
    public class Time
    {
        [DataMember]
        [Key]
        public int timeID { get; set; }

        [DataMember]
        [Required]
        [MaxLength(32)]
        [DefaultValue(null)]
        public String description { get; set; }
    }

    public class User
    {
        [DataMember]
        [Key]
        public int userID { get; set; }

        [DataMember]
        [MaxLength(32)]
        public String name { get; set; }

        [DataMember]
        [MaxLength(32)]
        public String surname { get; set; }
    }

    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        
    }
}
