using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models.Entities
{
    public class OgrenciListEntities
    {
        public int OgrenciNo { get; set; }
        public string OgrenciAdi { get; set; }
        public string OgrenciSoyadi { get; set; }
        public string OgrenciSifre { get; set; }
        public string OgrenciTelefon { get; set; }
        public string OgrenciMail { get; set; }
        public int OgrenciRolID { get; set; }
        public System.DateTime OgrenciUyelikTarihi { get; set; }
        public int OgrenciBolumID { get; set; }
        public Nullable<int> ProfilResimId { get; set; }
        public string OgrenciSinif { get; set; }
        public Nullable<bool> Aktiflik { get; set; }
        public string BolumAdi { get; set; }
        public string RolAdi { get; set; }
        public string ResimYolu { get; set; }
    }
}