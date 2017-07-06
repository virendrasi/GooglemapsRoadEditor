using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MapsAPI.Entities
{
    public class RoadMap
    {
        public int RoadId { get; set; }
        public string RoadName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Distance { get; set; }
        public bool IsDisable { get; set; }
    }
}