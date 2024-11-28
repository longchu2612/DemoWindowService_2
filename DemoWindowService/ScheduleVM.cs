﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWindowService
{
    public class ScheduleVM
    {
        public int id { get; set; }
        public DateTime date { get; set; }

        public int? FromX { get; set; }

        public int? FromY { get; set; }

        public int? ToX { get; set; }

        public int? ToY { get; set; }

        public string Reason { get; set; }
    }
}
