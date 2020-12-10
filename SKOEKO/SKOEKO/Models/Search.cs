using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKOEKO.Models
{
    public class Search
    {
        public string from { get; set; }
        public string to { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string MonthYear { get; set; }
    }
}