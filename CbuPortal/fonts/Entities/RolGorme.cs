using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models.Entities
{
    public class RolGorme
    {
        public string OgrenciMail { get; set; }
        public string OgrenciSifre { get; set; }
        public int OgrenciRolID { get; set; }
        public string AkademisyenSifre { get; set; }
        public string AkademisyenMail { get; set; }
        public int AkademisyenRolID { get; set; }
        public int AkademisyenId { get; set; }
        public string ProfilResmi { get; set; }
        public string OgrenciAdi { get; set; }
        public string AkademisyenAdi { get; set; }
        public string OgrenciSoyad { get; set; }
        public string AkademisyenSoyad { get; set; }
        public string RolAdi { get; set; }
        public int OgrenciNo { get; set; }


    }
}