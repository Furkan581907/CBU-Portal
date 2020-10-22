using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models.Entities
{
    public class MesajDetayEntities
    {
        public string name { get; set; }
        public int id { get; set; }
        public string image { get; set; }
        public string text { get; set; }
        public bool? okunduMu { get; set; }
    }
}