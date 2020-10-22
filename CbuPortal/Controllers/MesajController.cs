using CbuPortal.Models;
using CbuPortal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CbuPortal.Controllers
{
    public class MesajController : Controller
    {
        // GET: Mesaj
        cbuTezEntities icerik = new cbuTezEntities();
        public ActionResult MesajEkrani()
        {
            return View();
        }
        public ActionResult MesajGecmisi()
        {
            var list = icerik.Mesajlar.ToList();
            return View(list);
        }
        public string KullaniciId()
        {
            return Session["KullaniciId"].ToString();
        }
        public JsonResult AkademisyenleriGetir()
        {
            List<MesajDetayEntities> akademisyenler = new List<MesajDetayEntities>();
            List<Akademisyen> akademisyen = icerik.Akademisyen.ToList();
            foreach (var item in akademisyen)
            {
                MesajDetayEntities mesajDetay = new MesajDetayEntities();
                mesajDetay.id = item.AkademisyenId;
                mesajDetay.image = icerik.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == item.ProfilResimId).ProfilResmiYolu;
                mesajDetay.name = item.AkademisyenAdi + " " + item.AkademisyenSoyadi;
                int aliciId = Convert.ToInt32(Session["KullaniciId"]);
                Mesajlar mesajOkundu = icerik.Mesajlar.FirstOrDefault(s => s.AliciId == aliciId && s.GonderenId == item.AkademisyenId);
                if (mesajOkundu != null)
                    mesajDetay.okunduMu = mesajOkundu.OkunduMu;
                else
                    mesajDetay.okunduMu = false;

                Mesajlar mesaj = icerik.Mesajlar.FirstOrDefault(s => s.AliciId == aliciId);
                if (mesaj != null)
                {
                    mesajDetay.text = icerik.Mesajlar.FirstOrDefault(s => s.AliciId == aliciId).Mesaj;
                }
                else
                {
                    mesajDetay.text = " ";
                }
                akademisyenler.Add(mesajDetay);
            }

            return Json(akademisyenler, JsonRequestBehavior.AllowGet);
        }


        public string MesajAliciResmi(int? id)
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

        public string MesajAlicisiKim(int? id)
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
        public ActionResult MesajGonder(string mesaj,int aliciId,int gonderenId)
        {
            Mesajlar m = new Mesajlar();
            m.Mesaj = mesaj;
            m.GonderenId = gonderenId;
            m.MesajTarihi = DateTime.Now;
            m.AliciId = aliciId;
            m.OkunduMu = false;
            icerik.Mesajlar.Add(m);
            icerik.SaveChanges();

            return Json("Gönderildi");
        }

        [HttpPost]
        public ActionResult MesajlariGetir(int aliciId, int gonderenId)
        {
            List<Mesajlar> gidenmesajlar = icerik.Mesajlar.Where(s => s.AliciId == aliciId && s.GonderenId == gonderenId).Take(20).ToList();
            List<Mesajlar> gelenmesajlar = icerik.Mesajlar.Where(s => s.AliciId == gonderenId && s.GonderenId == aliciId).Take(20).ToList();
            List<MesajListEntities> mesajlar = new List<MesajListEntities>();

            foreach (var gelenitem in gidenmesajlar)
            {
                foreach (var gidenitem in gidenmesajlar)
                {
                    if (gelenitem.MesajTarihi>gidenitem.MesajTarihi)
                    {
                        MesajListEntities mesajList = new MesajListEntities();
                        mesajList.Mesaj = gelenitem.Mesaj;
                        mesajList.MesajTarihi = gelenitem.MesajTarihi;
                        mesajList.GonderenId = gelenitem.GonderenId;
                        mesajList.AliciId = gelenitem.AliciId;
                        mesajlar.Add(mesajList);
                        break;
                    }
                    else
                    {
                        MesajListEntities mesajList = new MesajListEntities();
                        mesajList.Mesaj = gidenitem.Mesaj;
                        mesajList.MesajTarihi = gidenitem.MesajTarihi;
                        mesajList.GonderenId = gidenitem.AliciId;
                        mesajList.AliciId = gidenitem.GonderenId;
                        mesajlar.Add(mesajList);
                        break;
                    }
                }
            }


            return Json(mesajlar);
        }

    }
}