using CbuPortal.AppClasses;
using CbuPortal.Models;
using RolGorme = CbuPortal.Models.RolGorme;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace CbuPortal.Controllers
{
    public class AkademisyenGirisController : Controller
    {
        cbuTezEntities icerik = new cbuTezEntities();
        HttpCookie akademisyencookie = new HttpCookie("CookieAka");
        // GET: AkademisyenGiris

        public ActionResult AkademisyenGirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AkademisyenGirisYap(Akademisyen akd)
        {
            akd.AkademisyenSifre = MD5Sifrele(akd.AkademisyenSifre);
            using (cbuTezEntities db = new cbuTezEntities())
            {
                var Akademisyen = db.Akademisyen.FirstOrDefault(x => x.AkademisyenMail == akd.AkademisyenMail && x.AkademisyenSifre == akd.AkademisyenSifre && x.KabulDurumu==true && x.Aktiflik==true);
                RolGorme data = new RolGorme();
                if (Akademisyen != null)
                {
                    string rolAdi = icerik.Rol.FirstOrDefault(s => s.RolID == Akademisyen.AkademisyenRolID).RolAdi;
                    string profilResmi = icerik.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == Akademisyen.ProfilResimId).ProfilResmiYolu;
                    RolGorme rolGormeData = new RolGorme
                    {
                        AkademisyenMail = Akademisyen.AkademisyenMail,
                        AkademisyenAdi = Akademisyen.AkademisyenAdi,
                        AkademisyenSoyad = Akademisyen.AkademisyenSoyadi,
                        ProfilResmi = profilResmi,
                        AkademisyenId = Akademisyen.AkademisyenId,
                        RolAdi = rolAdi,
                        AkademisyenSifre = Akademisyen.AkademisyenSifre,
                        Aktiflik = Akademisyen.Aktiflik,
                        AkademisyenKabul = Akademisyen.KabulDurumu,
                        

                    };
                    data = rolGormeData;
                    Session["KullaniciId"] = rolGormeData.AkademisyenId;
                    Session["KullaniciAdi"] = rolGormeData.AkademisyenAdi;
                    Session["KullaniciSoyad"] = rolGormeData.AkademisyenSoyad;
                    Session["KullaniciMail"] = rolGormeData.AkademisyenMail;
                    Session["rol"] = rolGormeData.RolAdi;
                    Session["ProfilResim"] = rolGormeData.ProfilResmi;
                    Session["KullaniciSifre"] = rolGormeData.AkademisyenSifre;
                    return RedirectToAction("Index", "Home");
                }
                return View(data);


            }
        }

        public ActionResult AkademisyenUyeOl()
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
        public ActionResult AkademisyenUyeOl(Akademisyen akd, HttpPostedFileBase file)
        {
            ProfilResimleri rsm = new ProfilResimleri();

            var kontrol = icerik.Akademisyen.FirstOrDefault(s => s.AkademisyenMail == akd.AkademisyenMail);
            if (kontrol == null)
            {
                akd.AkademisyenSifre = MD5Sifrele(akd.AkademisyenSifre);

                if (file != null)
                {

                    Image img = Image.FromStream(file.InputStream);
                    img.Save(Server.MapPath("/Theme/KullaniciResimleri/" + file.FileName));

                    rsm.ProfilResmiYolu = "/Theme/KullaniciResimleri/" + file.FileName;
                }
                else
                {
                    rsm.ProfilResmiYolu = "/Theme/KullaniciResimleri/hocaDefault.png";
                }



                Rol akademisyenRol = icerik.Rol.FirstOrDefault(x => x.RolAdi == "Akademisyen");
                icerik.ProfilResimleri.Add(rsm);
                icerik.SaveChanges();
                akd.ProfilResimId = rsm.ProfilResimID;
                akd.AkademisyenRolID = akademisyenRol.RolID;
                akd.AkademisyenUyelikTarihi = DateTime.Now;
                akd.KabulDurumu = false;
                akd.Aktiflik = true;
                icerik.Akademisyen.Add(akd);
                icerik.SaveChanges();



                return RedirectToAction("AkademisyenGirisYap");
            }
            else
            {
                //akademisyenUyeOl dan sonra , koyup tanım koyup fronttan onu çekip hata mesajı yazdırcaz.
                return RedirectToAction("AkademisyenUyeOl");
            }
        }


        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            if (Request.Cookies["CookieAka"] != null)
            {
                HttpCookie musteriCookie = Request.Cookies["CookieAka"];
                akademisyencookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(musteriCookie);
            }
            return RedirectToAction("AkademisyenGirisYap", "AkademisyenGiris");
        }


        public ActionResult AkademisyenWidget()
        {
            var data = icerik.Akademisyen.ToList();
            return View(data);
        }

        public ActionResult AkademisyenlerSayfasi()
        {

            var data = icerik.Akademisyen.ToList();
            var profilResimleri = icerik.ProfilResimleri.ToList();
            return View(data);
        }

        public string akademisyenlerSayfasiResim(int? id)
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

        public ActionResult AkademisyenSifreDegistir()
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
        public ActionResult AkademisyenSifreGuncelle(int id)
        {
            if (Session["rol"] != null)
            {
                string AkdemisyenSifre = Request["AkademisyenSifre"].ToString();
                string YeniSifre = Request["YeniSifre"].ToString();
                string YeniSifreTekrar = Request["YeniSifreTekrar"].ToString();
                var akaData = icerik.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
                var mevcut = icerik.Akademisyen.Find(akaData.AkademisyenId);
                if (mevcut.AkademisyenSifre != AkdemisyenSifre)
                {
                    TempData["Message"] = "Girdiğiniz şifre yanlış lütfen kontrol ediniz...";
                    return RedirectToAction("AkademisyenSifreDegistir", "AkademisyenGiris", TempData["Message"]);
                }
                else if (mevcut.AkademisyenSifre == AkdemisyenSifre && mevcut.AkademisyenSifre == YeniSifre)
                {
                    TempData["Message"] = "Girdiğiniz şifre yanlış lütfen kontrol ediniz...";
                    return RedirectToAction("AkademisyenSifreDegistir", "AkademisyenGiris", TempData["Message"]);
                }
                else if (mevcut.AkademisyenSifre == AkdemisyenSifre && mevcut.AkademisyenSifre == YeniSifreTekrar)
                {
                    TempData["Message"] = "Girdiğiniz şifre yanlış lütfen kontrol ediniz...";
                    return RedirectToAction("AkademisyenSifreDegistir", "AkademisyenGiris", TempData["Message"]);
                }
                else if (mevcut.AkademisyenSifre == AkdemisyenSifre && YeniSifre != YeniSifreTekrar)
                {
                    TempData["Message"] = "Girdiğiniz şifre yanlış lütfen kontrol ediniz...";
                    return RedirectToAction("AkademisyenSifreDegistir", "AkademisyenGiris", TempData["Message"]);
                }
                else if (mevcut.AkademisyenSifre == AkdemisyenSifre && YeniSifre == YeniSifreTekrar && AkdemisyenSifre != YeniSifre)
                {

                    mevcut.AkademisyenSifre = YeniSifre;
                    icerik.SaveChanges();
                }



                return RedirectToAction("AkademisyenSifreDegistir", "AkademisyenGiris", icerik.Akademisyen);
            }
            else
            {
                return RedirectToAction("KarsilamaEkrani", "Home");
            }

        }


        public ActionResult AkademisyenProfilIncele()
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

        public ActionResult AkademisyenProfilGuncelleSayfasi()
        {
            if (Session["rol"].ToString() != null && Session["rol"].ToString() == "Akademisyen")
            {
                return View();
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
        }

        [HttpPost]
        public ActionResult AkademisyenProfilGuncelle(int id)
        {
            if (Session["rol"] != null)
            {
                string akademisyenAdi = Request["akademisyenAdi"].ToString();
                string akaSoyadi = Request["akademisyenSoyadi"].ToString();
                string akaMail = Request["akademisyenMail"].ToString();
                var akaData = icerik.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
                var mevcut = icerik.Akademisyen.Find(akaData.AkademisyenId);
                mevcut.AkademisyenAdi = akademisyenAdi;
                mevcut.AkademisyenSoyadi = akaSoyadi;
                mevcut.AkademisyenMail = akaMail;
                icerik.SaveChanges();
                return RedirectToAction("CikisYap", "OgrenciGiris", icerik.Ogrenci);
            }
            else
            {
                return RedirectToAction("KarsilamaEkrani", "Home");
            }
        }

    }

}