using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbuPortal.Models
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
        public int? sayfaNo { get; set; }
        public string OgrenciBolum { get; set; }
        public string OgrenciSinif { get; set; }
        public string CalistigiYer { get; set; }
        public string CalistigiPozisyon { get; set; }
        public string CalistigiSehir { get; set; }
        public string AkademisyenBolum { get; set; }
        public string OgrenciTelefon { get; set; }
        public string InstagramAdres { get; set; }
        public string FacebookAdres { get; set; }
        public string LinkedinAdres { get; set; }
        public string DigerAdres { get; set; }
        public string KullaniciId { get; set; }
        public bool? Aktiflik { get; set; }
        public bool? AkademisyenKabul { get; set; }
        public string AdminMail { get; set; }

    }
}