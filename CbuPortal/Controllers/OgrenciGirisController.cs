using CbuPortal.AppClasses;
using CbuPortal.Models;
using RolGorme = CbuPortal.Models.RolGorme;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace CbuPortal.Controllers
{
    public class OgrenciGirisController : Controller
    {

        cbuTezEntities icerik = new cbuTezEntities();
        HttpCookie ogrenciCookie = new HttpCookie("CookieOgr");
        ProfilResimleri prf = new ProfilResimleri();
        // GET: OgrenciGiris

        public ActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Ogrenci ogr)
        {
            ogr.OgrenciSifre = MD5Sifrele(ogr.OgrenciSifre);
            using (cbuTezEntities db = new cbuTezEntities())
            {
                var ogrenci = db.Ogrenci.FirstOrDefault(x => x.OgrenciMail == ogr.OgrenciMail && x.OgrenciSifre == ogr.OgrenciSifre && x.Aktiflik==true);
                RolGorme data = new RolGorme();
                if (ogrenci != null)
                {
                    string rolAdi = icerik.Rol.FirstOrDefault(s => s.RolID == ogrenci.OgrenciRolID).RolAdi;
                    string bolumAdi = icerik.Bolumler.FirstOrDefault(s => s.BolumID == ogrenci.OgrenciBolumID).BolumAdi;
                    var kisiselDetay = icerik.KisiselDetay.FirstOrDefault(s => s.KullaniciId == ogrenci.OgrenciNo);
                    string profilResmi = icerik.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == ogrenci.ProfilResimId).ProfilResmiYolu;
                    var datam = icerik.SosyalMedya.FirstOrDefault(x => x.KullaniciId == ogrenci.OgrenciNo);
                    string facebookAdres = "";
                    string linkedinAdres = "";
                    string instagramAdres = "";
                    string DigerAdres = "";
                    if (datam != null)
                    {
                        facebookAdres = datam.SosyalMedyaFacebook;
                        linkedinAdres = datam.SosyalMedyaLinkedin;
                        instagramAdres = datam.SosyalMedyaInstagram;
                        DigerAdres = datam.DigerSite;
                    }
                    string CalistigiFirma = "", calistigiBolum = "", CalistigiIl = "", OgrenciTelefon = "";
                    var firmaBilgi = db.KisiselDetay.FirstOrDefault(s => s.KullaniciId == ogrenci.OgrenciNo);
                    if (firmaBilgi != null)
                    {
                        CalistigiFirma = firmaBilgi.CalistigiFirma;
                        calistigiBolum = firmaBilgi.Bolumu;
                        CalistigiIl = firmaBilgi.CalistigiFirmaninIli;
                    }


                    RolGorme rolGormeData = new RolGorme
                    {
                        OgrenciMail = ogrenci.OgrenciMail,
                        OgrenciAdi = ogrenci.OgrenciAdi,
                        OgrenciSoyad = ogrenci.OgrenciSoyadi,
                        ProfilResmi = profilResmi,
                        OgrenciNo = ogrenci.OgrenciNo,
                        RolAdi = rolAdi,
                        OgrenciBolum = bolumAdi,
                        OgrenciSinif = ogrenci.OgrenciSinif,
                        CalistigiYer = CalistigiFirma,
                        CalistigiPozisyon = calistigiBolum,
                        CalistigiSehir = CalistigiIl,
                        OgrenciTelefon = ogrenci.OgrenciTelefon,
                        LinkedinAdres = linkedinAdres,
                        InstagramAdres = instagramAdres,
                        FacebookAdres = facebookAdres,
                        KullaniciId = ogrenci.OgrenciNo.ToString(),
                        Aktiflik = ogrenci.Aktiflik
                    };
                    data = rolGormeData;
                    Session["Aktiflik"] = rolGormeData.Aktiflik;
                    Session["KullaniciId"] = rolGormeData.OgrenciNo;
                    Session["KullaniciAdi"] = rolGormeData.OgrenciAdi;
                    Session["KullaniciSoyad"] = rolGormeData.OgrenciSoyad;
                    Session["KullaniciMail"] = rolGormeData.OgrenciMail;
                    Session["rol"] = rolGormeData.RolAdi;
                    Session["ProfilResim"] = rolGormeData.ProfilResmi;
                    Session["KullaniciSifre"] = rolGormeData.OgrenciSifre;
                    Session["Bolum"] = rolGormeData.OgrenciBolum;
                    Session["CalistigiYer"] = rolGormeData.CalistigiYer;
                    Session["CalistigiPozisyon"] = rolGormeData.CalistigiPozisyon;
                    Session["CalistigiSehir"] = rolGormeData.CalistigiSehir;
                    Session["Telefon"] = rolGormeData.OgrenciTelefon;
                    Session["OgrenciSinif"] = rolGormeData.OgrenciSinif;
                    Session["BolumAdi"] = bolumAdi;
                    Session["LinkedinAdres"] = linkedinAdres;
                    Session["InstagramAdres"] = instagramAdres;
                    Session["FacebookAdres"] = facebookAdres;
                    Session["DigerAdres"] = DigerAdres;
                    return RedirectToAction("Index", "Home");
                }
                return View(data);


            }
        }



        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            if (Request.Cookies["ogrenciCookie"] != null)
            {
                HttpCookie ogrCookie = Request.Cookies["ogrenciCookie"];
                ogrenciCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ogrCookie);
            }
            return RedirectToAction("KarsilamaEkrani", "Home");
        }

        public ActionResult UyeOl()
        {
            ViewBag.Bolumler = icerik.Bolumler.ToList();
            return View();
        }



        public static string MD5Sifrele(string sifrelenecekMetin)
        {

            // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            //dizinin hash'ini hesaplattık.
            dizi = md5.ComputeHash(dizi);
            //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
            StringBuilder sb = new StringBuilder();
            //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürdük.
            return sb.ToString();
        }


        [HttpPost]
        public ActionResult UyeOl(Ogrenci ogr, Biografi bio, KisiselDetay kd, SosyalMedya sm, HttpPostedFileBase file)
        {
            try
            {
                ProfilResimleri rsm = new ProfilResimleri();

                ogr.OgrenciSifre = MD5Sifrele(ogr.OgrenciSifre);



                if (file != null)
                {

                    Image img = Image.FromStream(file.InputStream);

                    img.Save(Server.MapPath("/Theme/KullaniciResimleri/" + file.FileName));
                    rsm.ProfilResmiYolu = "/Theme/KullaniciResimleri/" + file.FileName;
                }
                else
                {
                    rsm.ProfilResmiYolu = "/Theme/KullaniciResimleri/ogrenciDefault.jpg";
                }





                icerik.ProfilResimleri.Add(rsm);
                icerik.SaveChanges();
                ogr.ProfilResimId = rsm.ProfilResimID;



                if (Request["OgrenciSinif"].ToString() == "Mezun")
                {
                    ogr.OgrenciRolID = 7;
                }
                else if (Request["OgrenciSinif"].ToString() != "Mezun" && Request["OgrenciSinif"].ToString() != "0")
                {
                    ogr.OgrenciRolID = 6;
                    ogr.OgrenciSinif = Request["OgrenciSinif"].ToString();
                }
                else
                {
                    ViewBag["Hata"] = "Hata";
                }

                ogr.OgrenciUyelikTarihi = DateTime.Now;
                ogr.Aktiflik = true;
                icerik.Ogrenci.Add(ogr);
                sm.KullaniciId = ogr.OgrenciNo;
                kd.KullaniciId = ogr.OgrenciNo;
                icerik.Biografi.Add(bio);
                icerik.SosyalMedya.Add(sm);
                icerik.KisiselDetay.Add(kd);
                icerik.SaveChanges();

                return RedirectToAction("GirisYap");
            }
            catch (Exception ex)
            {
                return RedirectToAction(ex.ToString());
            }
        }
        public ActionResult OgrenciSifreDegistirSayfasi()
        {
            if (Session["rol"].ToString() != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }

        }
        [HttpPost]

        public ActionResult OgrenciSifreDegistir(int id)
        {
            if (Session["rol"] != null)
            {
                string OgrenciSifre = MD5Sifrele(Request["OgrenciSifre"].ToString());
                string YeniSifre = MD5Sifrele(Request["YeniSifre"].ToString());
                string YeniSifreTekrar = MD5Sifrele(Request["YeniSifreTekrar"].ToString());
                var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
                var mevcut = icerik.Ogrenci.Find(ogrData.OgrenciNo);
                if (mevcut.OgrenciSifre != OgrenciSifre)
                {
                    TempData["Message"] = "Girdiğiniz şifre yanlış lütfen kontrol ediniz...";
                    return RedirectToAction("OgrenciSifreDegistirSayfasi", "OgrenciGiris", TempData["Message"]);
                }
                else if (mevcut.OgrenciSifre == OgrenciSifre && mevcut.OgrenciSifre == YeniSifre)
                {
                    ViewBag.Message = "Aynı Şifreyi Girmeyiniz.";
                }
                else if (mevcut.OgrenciSifre == OgrenciSifre && mevcut.OgrenciSifre == YeniSifreTekrar)
                {
                    ViewBag.Message = "Aynı Şifreyi Girmeyiniz.";
                }
                else if (mevcut.OgrenciSifre == OgrenciSifre && YeniSifre != YeniSifreTekrar)
                {
                    ViewBag.Message = "Girdiğiniz Yeni Parolaları tekrar ediniz";
                }
                else if (mevcut.OgrenciSifre == OgrenciSifre && YeniSifre == YeniSifreTekrar && OgrenciSifre != YeniSifre)
                {

                    mevcut.OgrenciSifre = YeniSifre;
                    icerik.SaveChanges();
                }



                return RedirectToAction("CikisYap", "OgrenciGiris", icerik.Ogrenci);
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }

        }

        [HttpPost]
        public ActionResult OgrenciProfilGuncelle(int id)
        {
            if (Session["rol"] != null)
            {
                string ogrenciAdi = Request["ogrenciAdi"].ToString();
                string ogrenciSoyadi = Request["ogrenciSoyadi"].ToString();
                string ogrenciMail = Request["ogrenciMail"].ToString();
                var ogrData = icerik.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
                var mevcut = icerik.Ogrenci.Find(ogrData.OgrenciNo);
                mevcut.OgrenciAdi = ogrenciAdi;
                mevcut.OgrenciSoyadi = ogrenciSoyadi;
                mevcut.OgrenciMail = ogrenciMail;
                icerik.SaveChanges();
                return RedirectToAction("CikisYap", "OgrenciGiris", icerik.Ogrenci);
            }
            else
            {
                return RedirectToAction("KarsilamaEkrani", "Home");
            }
        }
        public ActionResult OgrenciProfilGuncelleSayfasi()
        {
            if (Session["rol"].ToString() != null && Session["rol"].ToString() != "Akademisyen")
            {
                return View();
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
        }
        public ActionResult OgrenciProfilDetay(Ogrenci ogr)
        {
            if (Session["rol"].ToString() != null && Session["rol"].ToString() != "Akademisyen")
            {
                return View();
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
        }

        //Bu tamamen hazır sadece görünüm sayfası eklenecek silme.

        public ActionResult MezunOgrenciListele()
        {
            if (Session["rol"] != null)
            {
                var data = icerik.Ogrenci.Where(x => x.OgrenciRolID == 7).ToList();
                return View(data);
            }
            else
                return RedirectToAction("CikisYap", "OgrenciGiris");

        }
        public string MezunlarSayfasiResim(int? id)
        {
            string sonuc = "";
            var profilResimleri = icerik.ProfilResimleri.ToList();

            foreach (ProfilResimleri item in profilResimleri)
            {
                if (Convert.ToInt32(id) == item.ProfilResimID)
                {
                    sonuc = item.ProfilResmiYolu;
                    break;
                }
            }
            return sonuc;
        }

        public ActionResult DosyaGonder(int id, HttpPostedFileBase file)
        {
            Dosyalar d = new Dosyalar();
            DosyaGonderimleri dg = new DosyaGonderimleri();


            d.DosyaAciklamasi = Request["dosyaAciklama"].ToString();
            d.DosyaYuklemeTarihi = DateTime.Now;
            d.DosyaDuyuruID = Convert.ToInt32(Request["DosyaDuyuruId"]);
            var path = Path.Combine(Server.MapPath("/Dosyalar/DuyuruDosyalari/"), file.FileName);
            file.SaveAs(path);
            d.DosyaYolu = "/Dosyalar/DuyuruDosyalari/" + file.FileName;
            d.DosyaSahibiID = Convert.ToInt32(Session["KullaniciId"]);
            icerik.Dosyalar.Add(d);
            icerik.SaveChanges();
            dg.DosyayiAlanID = id;
            dg.DosyayıGonderenID = Convert.ToInt32(Session["KullaniciId"]);
            dg.GonderilenDosyaID = d.DosyaID;
            icerik.DosyaGonderimleri.Add(dg);
            icerik.SaveChanges();

            return RedirectToAction("Index", "Home");

        }
    }

}
