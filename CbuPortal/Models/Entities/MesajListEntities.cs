using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models.Entities
{
    public class MesajListEntities
    {
        public int Id { get; set; }
        public int GonderenId { get; set; }
        public int AliciId { get; set; }
        public string Mesaj { get; set; }
        public string MesajKonusu { get; set; }
        public Nullable<System.DateTime> MesajTarihi { get; set; }
        public string GonderenAdi { get; set; }
        public string AliciAdi { get; set; }
        public bool OkunduMu { get; set; }
    }
}