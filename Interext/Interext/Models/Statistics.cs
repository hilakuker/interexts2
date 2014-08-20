using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class Statistics
    {
        public List<StatisticItem> Gender { get; set; }

        public List<StatisticItem> Age { get; set; }

        public List<StatisticItem> Interests { get; set; }
    }

    public class StatisticItem
    {
        public int number { get; set; }
        public string title { get; set; }
    }
}