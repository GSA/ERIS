﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    public class Position
    {
        public string JobTitle { get; set; }
        public string Region { get; set; }
        public bool? IsVirtual { get; set; }
        public string VirtualRegion { get; set; }
        public string OfficeSymbol { get; set; }
        public string MajorOrg { get; set; }       
    }
}
