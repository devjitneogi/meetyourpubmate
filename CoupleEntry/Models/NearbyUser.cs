using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoupleEntry.Models
{
    public class NearbyUser
    {
        public string uname { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime lastSeen { get; set; }
    }
}