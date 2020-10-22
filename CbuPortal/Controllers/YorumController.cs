using CbuPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CbuPortal.Controllers
{
    public class YorumController : Controller
    {
        cbuTezEntities icerik = new cbuTezEntities();
        // GET: Yorum

        public string YorumYapaninResmi(int? id)
        {

            var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
            var akaData = icerik.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);


            if (ogrData != null)
            {
                var data = icerik.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == ogrData.ProfilResimId);
                return data.ProfilResmiYolu;
            }
            else if (akaData != null)
            {
                var data = icerik.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == akaData.ProfilResimId);
                return data.ProfilResmiYolu;
            }
            else
                return "";


        }
        public string YorumYapan(int? id)
        {

            var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
            var akaData = icerik.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
            if (ogrData != null)
            {
                return ogrData.OgrenciAdi + " " + ogrData.OgrenciSoyadi;
            }
            if (akaData != null)
            {
                return akaData.AkademisyenAdi + " " + akaData.AkademisyenSoyadi;
            }
            else
                return "";


        }

        [HttpPost]
        public ActionResult YorumYap(int id)
        {
            Yorum y = new Yorum();
            y.Yorum1 = Request["Yorum1"].ToString();
            y.YorumYapanId = Convert.ToInt32(Session["KullaniciId"]);
            y.YorumTarih = DateTime.Now;
            y.YorumDurumu = true;
            y.GonderiId = id;
            icerik.Yorum.Add(y);
            icerik.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult YorumlariListele(int id)
        {
            var data = icerik.Yorum.Where(s => s.GonderiId == id).Where(y => y.YorumDurumu == true).ToList();
            ViewData["id"] = id;
            return View(data);
        }

        public int YorumSayisi(int id)
        {
            var data = icerik.Yorum.Where(s => s.GonderiId == id).ToList();

            int sayi = data.Count();

            return sayi;
        }

        public ActionResult YorumSil(int id)
        {

            if (Session["rol"].ToString() != null)
            {
                string KullaniciId = Session["KullaniciId"].ToString();
                var silinecekYorum = icerik.Yorum.FirstOrDefault(s => s.Id == id && s.YorumYapanId.ToString() == KullaniciId);
                silinecekYorum.YorumDurumu = false;
                icerik.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
                return RedirectToAction("KarsilamaEkrani", "Home");

        }

    }
}