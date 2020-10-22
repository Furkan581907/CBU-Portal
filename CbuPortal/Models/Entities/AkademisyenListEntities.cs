using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models.Entities
{
    public class AkademisyenListEntities
    {
        public string AkademisyenSifre { get; set; }
        public string AkademisyenMail { get; set; }
        public int? AkademisyenRolID { get; set; }
        public int AkademisyenId { get; set; }
        public string AkademisyenAdi { get; set; }
        public string AkademisyenSoyad { get; set; }
        public string AkademisyenBolumAdi { get; set; }
        public int? AkademisyenBolumId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string ResimYolu { get; set; }
        public string RolAdi { get; set; }
        public bool? KabulDurumu { get; set; }
        public bool? Aktiflik { get; set; }

    }
}