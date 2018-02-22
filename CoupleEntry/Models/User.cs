﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoupleEntry.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public string EmailId { get; set; }
        public int Age { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime LastSeen { get; set; }
    }
}