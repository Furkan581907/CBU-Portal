using CbuPortal.AppClasses;
using CbuPortal.Models;
using RolGorme = CbuPortal.Models.RolGorme;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CbuPortal.Controllers
{
    public class DuyuruController : Controller
    {

        cbuTezEntities icerik = new cbuTezEntities();


        public ActionResult DuyuruDetay(int id)
        {

            var data = icerik.Duyurular.FirstOrDefault(x => x.Id == id);
            if (data.DuyuruDurum != false)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("PageError", "Error");
            }

        }
        public ActionResult SonBegenilenler()
        {
            int id = Convert.ToInt32(Session["KullaniciId"]);
            if (Session["rol"] != null)
            {
                string rolAdi = Session["rol"].ToString();
                string profilResmi = Session["ProfilResim"].ToString();


                var data = icerik.Favoriler.Where(s => s.FavoriSahibiId == id).ToList();
                List<Duyurular> d = new List<Duyurular>();
                foreach (var datam in data)
                {
                    var search = icerik.Duyurular.Where(s => s.Id == datam.FavDuyuruId).FirstOrDefault();
                    d.Add(search);
                }

                return View(d);
            }
            else
            {
                return RedirectToAction("PageError", "Error");

            }
        }

            public ActionResult EnSonIsDuyurulari()
        {
            var data = icerik.Duyurular.Where(x=>x.DuyuruTipi=="8").OrderByDescending(s => s.DuyuruTarihi).Take(3).ToList();
            return View(data);
        }
        public ActionResult IsDuyurulariSayfasi()
        {
            var data = icerik.Duyurular.Where(x => x.DuyuruTipi == "8").OrderByDescending(s => s.DuyuruTarihi).ToList();
            return View(data);
        }



        public ActionResult DuyuruWidget()
        {
            string KullaniciId = Session["KullaniciId"].ToString();
            int ogrNo = Convert.ToInt32(Session["KullaniciId"].ToString());
            var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == ogrNo);
            string rolAdi = Session["rol"].ToString();
            string profilResmi = Session["ProfilResim"].ToString();
            var data = icerik.Duyurular.Where(x => x.DuyuruTipi == "6").OrderByDescending(s => s.DuyuruTarihi).Take(3).ToList();
            return View(data);
        }
        public ActionResult OdevDuyuruListele()
        {
            var data = icerik.Duyurular.Where(x => x.DuyuruTipi == "6").Where(y => y.DuyuruDurum == true).ToList();
            data = data.OrderBy(x => x.DuyuruTarihi.Value.TimeOfDay.Days).Reverse().ToList();
            return View(data);
        }

        public ActionResult DuyuruListele()
        {

            string KullaniciId = Session["KullaniciId"].ToString();
            int ogrNo = Convert.ToInt32(Session["KullaniciId"].ToString());
            var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == ogrNo);
            string rolAdi = Session["rol"].ToString();
            string profilResmi = Session["ProfilResim"].ToString();


            var data = icerik.Duyurular.ToList();
            data = data.OrderBy(x => x.DuyuruTarihi.Value.TimeOfDay.Days).Reverse().ToList();

            return View(data);
        }

        public string DuyuruYapan(int? id)
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

        public string DuyuruYapaninResmi(int? id)
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


        public int DuyurununKaydedilmeSayisi(int id)
        {
            var data = icerik.Favoriler.Where(s => s.FavDuyuruId == id).ToList();

            int sayi = data.Count();

            return sayi;
        }

        public ActionResult EnSonDuyurular()
        {
            var data = icerik.Duyurular.OrderByDescending(s => s.DuyuruTarihi).Take(6).ToList();
            return View(data);
        }

        public ActionResult DuyuruEkle()
        {
            var data = icerik.DuyuruTipleri.ToList();
            return View(data);
        }

        public ActionResult FavorilereEkle(int id)
        {
            if (Session["rol"].ToString() != null)
            {
                int KullaniciId = Convert.ToInt32(Session["KullaniciId"].ToString());
                var kontrol = icerik.Favoriler.FirstOrDefault(s => s.FavDuyuruId == id && s.FavoriSahibiId == KullaniciId);
                if (kontrol == null)
                {
                    Favoriler fav = new Favoriler();
                    fav.FavDuyuruId = id;
                    string rolAdi = Session["rol"].ToString();
                    string profilResmi = Session["ProfilResim"].ToString();
                    fav.FavoriSahibiId = KullaniciId;
                    fav.FavZamani = DateTime.Now;
                    icerik.Favoriler.Add(fav);
                    icerik.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    ViewBag.Message = "Zaten ekli";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            else
                return RedirectToAction("KarsilamaEkrani", "Home");
        }

        public ActionResult FavorileriListele(int id)
        {

            if (Session["rol"] != null)
            {
                string rolAdi = Session["rol"].ToString();
                string profilResmi = Session["ProfilResim"].ToString();


                var data = icerik.Favoriler.Where(s => s.FavoriSahibiId == id).ToList();
                List<Duyurular> d = new List<Duyurular>();
                foreach (var datam in data)
                {
                    var search = icerik.Duyurular.Where(s => s.Id == datam.FavDuyuruId).FirstOrDefault();
                    d.Add(search);
                }

                return View(d);

            }
            else
                return RedirectToAction("KarsilamaEkrani", "Home");


        }

        public ActionResult FavoriKaldir(int id)
        {
            string KullaniciId = Session["KullaniciId"].ToString();
            string rolAdi = Session["rol"].ToString();
            var favlanan = icerik.Favoriler.FirstOrDefault(s => s.FavDuyuruId == id && s.FavoriSahibiId.ToString() == KullaniciId);
            icerik.Favoriler.Remove(favlanan);
            icerik.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult DuyuruyuSil(int id)
        {
            if (Session["rol"].ToString() != null)
            {
                string KullaniciId = Session["KullaniciId"].ToString();
                string rolAdi = Session["rol"].ToString();
                var silinecekDuyuru = icerik.Duyurular.FirstOrDefault(s => s.Id == id && s.DuyuruyuYapanId.ToString() == KullaniciId);
                silinecekDuyuru.DuyuruDurum = false;
                icerik.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            else
                return RedirectToAction("KarsilamaEkrani", "Home");
        }


        [HttpPost]
        public ActionResult DuyuruEkle(Duyurular d, HttpPostedFileBase file, string yorumDurumu)
        {
            try
            {
                if (Session["rol"].ToString() != "Ogrenci" && Session["rol"] != null)
                {

                    string KullaniciId = Session["KullaniciId"].ToString();
                    string rolAdi = Session["rol"].ToString();
                    string profilResmi = Session["ProfilResim"].ToString();
                    if (d.Aciklama != null && d.Baslik != null)
                    {
                        if (file != null)
                        {
                            Image img = Image.FromStream(file.InputStream);

                            img.Save(Server.MapPath("/Theme/DuyuruResimleri/" + file.FileName));

                            d.DuyuruResmi = "/Theme/DuyuruResimleri/" + file.FileName;
                        }
                        else
                        {
                            if (rolAdi == "Akademisyen")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuru.jpg";
                            }
                            else if (rolAdi == "Mezun")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuruOgr.jpg";
                            }

                        }

                        if (!string.IsNullOrEmpty(yorumDurumu))
                        {
                            if (yorumDurumu == "Açık")
                            {
                                d.DuyuruYorumaAcikMi = true;
                            }
                        }
                        else if (!string.IsNullOrEmpty(yorumDurumu))
                        {
                            if (yorumDurumu == "Kapalı")
                            {
                                d.DuyuruYorumaAcikMi = false;
                            }

                        }

                        d.DuyuruyuYapanId = Convert.ToInt32(KullaniciId);
                        d.DuyuruTarihi = DateTime.Now;
                        d.DuyuruDurum = true;
                        icerik.Duyurular.Add(d);
                        icerik.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string hata = "Duyurunuz Eklenemedi";
                        return View(hata);
                    }
                }
                else
                    return RedirectToAction("KarsilamaEkrani", "Home");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        [HttpPost]
        public ActionResult OdevDuyuruEkle(Duyurular d, HttpPostedFileBase file, string yorumDurumu)
        { 
            try
            {
                if (Session["rol"].ToString() != "Ogrenci" && Session["rol"] != null)
                {

                    string KullaniciId = Session["KullaniciId"].ToString();
                    string rolAdi = Session["rol"].ToString();
                    string profilResmi = Session["ProfilResim"].ToString();
                    if (d.Aciklama != null && d.Baslik != null)
                    {
                        if (file != null)
                        {
                            Image img = Image.FromStream(file.InputStream);

                            img.Save(Server.MapPath("/Theme/DuyuruResimleri/" + file.FileName));

                            d.DuyuruResmi = "/Theme/DuyuruResimleri/" + file.FileName;
                        }
                        else
                        {
                            if (rolAdi == "Akademisyen")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuru.jpg";
                            }
                            else if (rolAdi == "Mezun")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuruOgr.jpg";
                            }

                        }

                        if (!string.IsNullOrEmpty(yorumDurumu))
                        {
                            if (yorumDurumu == "Açık")
                            {
                                d.DuyuruYorumaAcikMi = true;
                            }
                        }
                        else if (!string.IsNullOrEmpty(yorumDurumu))
                        {
                            if (yorumDurumu == "Kapalı")
                            {
                                d.DuyuruYorumaAcikMi = false;
                            }

                        }

                        d.DuyuruyuYapanId = Convert.ToInt32(KullaniciId);
                        d.DuyuruTarihi = DateTime.Now;
                        d.DuyuruDurum = true;
                        icerik.Duyurular.Add(d);
                        icerik.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string hata = "Duyurunuz Eklenemedi";
                        return View(hata);
                    }
                }
                else
                    return RedirectToAction("KarsilamaEkrani", "Home");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult OdevDuyuruEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DuyuruEkleis(Duyurular d, HttpPostedFileBase file)
        {
            try
            {
                if (Session["rol"].ToString() != "Ogrenci" && Session["rol"] != null)
                {
                    IsIlani ilan = new IsIlani(); 
                    string KullaniciId = Session["KullaniciId"].ToString();
                    string rolAdi = Session["rol"].ToString();
                    string profilResmi = Session["ProfilResim"].ToString();
                    if (d.Aciklama != null && d.Baslik != null)
                    {
                        if (file != null)
                        {
                            Image img = Image.FromStream(file.InputStream);

                            img.Save(Server.MapPath("/Theme/DuyuruResimleri/" + file.FileName));

                            d.DuyuruResmi = "/Theme/DuyuruResimleri/" + file.FileName;
                        }
                        else
                        {
                            if (rolAdi == "Akademisyen")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuru.jpg";
                            }
                            else if (rolAdi == "Mezun")
                            {
                                d.DuyuruResmi = "/Theme/img/duyuruOgr.jpg";
                            }

                        }
                        
                        d.DuyuruYorumaAcikMi = true;
                        d.DuyuruyuYapanId = Convert.ToInt32(KullaniciId);
                        d.DuyuruTarihi = DateTime.Now;
                        d.DuyuruDurum = true;
                        icerik.Duyurular.Add(d);
                        ilan.DuyuruId = d.Id;
                        ilan.SirketAdi = Request["SirketAdi"].ToString();
                        ilan.SirketPozisyonu = Request["SirketPozisyonu"].ToString(); ;
                        ilan.SirketSehir = Request["SirketSehir"].ToString(); ;
                        icerik.IsIlani.Add(ilan);
                        icerik.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string hata = "Duyurunuz Eklenemedi";
                        return View(hata);
                    }
                }
                else
                    return RedirectToAction("KarsilamaEkrani", "Home");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult DuyuruEkleis()
        {

            return View();
        }
        public ActionResult AramaSonucu()
        {   var gelenDeger = Request["search"].ToString();
            var deneme = from m in icerik.Duyurular.Where(x=>x.DuyuruDurum==true) select m;

            if (gelenDeger==null || gelenDeger == " " || gelenDeger =="")
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                deneme = deneme.Where(s => s.Aciklama.Contains(gelenDeger)|| s.Baslik.Contains(gelenDeger));
                
                return View(deneme.ToList());
            }

         
        }

        public ActionResult AramaSonucuAkademisyen()
        {
            var gelenDeger = Request["searchAka"].ToString();
            var deneme = from m in icerik.Akademisyen select m;

            if (gelenDeger == null || gelenDeger == " " || gelenDeger == "")
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                deneme = deneme.Where(s => s.AkademisyenAdi.Contains(gelenDeger) || s.AkademisyenSoyadi.Contains(gelenDeger));

                return View(deneme.ToList());
            }


        }


        //public ActionResult AramaSonucuFavoriler()
        //{
        //    var gelenDeger = Request["searchFav"].ToString();
        //    var res = from ord in icerik.Favoriler
        //              join du  in icerik.Duyurular
        //                   on ord.FavDuyuruId equals du.Id into g
        //              from d in g.DefaultIfEmpty()
        //              select new
        //              {
        //                  g.Where(g=>g.Aciklama.Contains(gelenDeger))
        //              };
        //}

        //public ActionResult ProfilGosterBegenilenler(int id)
        //{
        //    if (Session["rol"] != null)
        //    {
        //        string rolAdi = Session["rol"].ToString();
        //        string profilResmi = Session["ProfilResim"].ToString();


        //        var data = icerik.Favoriler.Where(s => s.FavoriSahibiId == id).ToList();
        //        List<Duyurular> d = new List<Duyurular>();
        //        foreach (var datam in data)
        //        {
        //            var search = icerik.Duyurular.Where(s => s.Id == datam.FavDuyuruId).FirstOrDefault();
        //            d.Add(search);
        //        }

        //        return View(d);
        //    }
        //    else
        //    {
        //        return RedirectToAction("PageError", "Error");

        //    }
        //}



        public ActionResult AramaSonucuIs()
        {
            var gelenDeger = Request["searchIs"].ToString();
            var deneme = from m in icerik.Duyurular.Where(x=>x.DuyuruTipi=="8") select m;

            if (gelenDeger == null || gelenDeger == " " || gelenDeger == "")
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                deneme = deneme.Where(s => s.Aciklama.Contains(gelenDeger) || s.Baslik.Contains(gelenDeger));

                return View(deneme.ToList());
            }


        }

    }
}
