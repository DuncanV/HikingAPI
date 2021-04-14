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
}
