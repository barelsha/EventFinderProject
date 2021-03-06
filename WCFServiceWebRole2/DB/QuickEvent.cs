﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFServiceWebRole2.DB
{
    public class QuickEvent
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
        public int UserID { get; set; }
        public int? Type { get; set; }
    }
}