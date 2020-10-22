using CbuPortal.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RolGorme = CbuPortal.Models.RolGorme;

namespace CbuPortal.Controllers
{
    public class HomeController : Controller
    {


        cbuTezEntities db = new cbuTezEntities();
        public ActionResult AnaSayfaSosyal()
        {
            var veri = db.Duyurular.Where(y => y.DuyuruDurum == true).OrderByDescending(m => m.DuyuruTarihi).Take(5).ToList();
            if (Session["rol"].ToString() != "Mezun")
            {
                 veri = db.Duyurular.Where(y => y.DuyuruDurum == true).OrderByDescending(m => m.DuyuruTarihi).Take(5).ToList();

            }
            else
            {
                veri = db.Duyurular.Where(x => x.DuyuruTipi == "7" || x.DuyuruTipi == "8").Where(y => y.DuyuruDurum == true).OrderByDescending(m => m.DuyuruTarihi).Take(5).ToList();

            }
            return View(veri);
        }
        public ActionResult IsIlanDetay(int? id)
        {
            var ilan = db.IsIlani.Where(x => x.DuyuruId == id).ToList();
            return View(ilan);
        }
        public string DuyuruTuru(int id)
        {
            var data = db.Duyurular.FirstOrDefault(x => x.Id == id).DuyuruTipi;
            if (data == "6")
            {
                ViewData["DuyuruTuru"] = "Ödev Duyurusu";
            }
            else if (data=="8")
            {
                ViewData["DuyuruTuru"] = "İş Duyurusu";

            }
            else
            {
                ViewData["DuyuruTuru"] = "Genel Duyuru";
            }

            return ViewData["DuyuruTuru"].ToString();

        }


        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
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


        public ActionResult KarsilamaEkrani()
        {
            return View();
        }
        public ActionResult SolMenu()
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
        public ActionResult ProfilGoster(int id)

        {
            TempData["baskasininProfili"] = id;
            var AkaData = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == id);
            var ogrData = db.Ogrenci.FirstOrDefault(y => y.OgrenciNo == id);

            if (id == Convert.ToInt32(Session["KullaniciId"]))
            {
                return RedirectToAction("Profilim", "Home");
            }
            else if (AkaData != null)
            {
                var profilResim = db.ProfilResimleri.FirstOrDefault(a => a.ProfilResimID == AkaData.ProfilResimId).ProfilResmiYolu;
                ViewData["ProfilGosterResim"] = profilResim;
                var akaMevcut = db.Akademisyen.Find(AkaData.AkademisyenId);
                var rolGetir = db.Rol.FirstOrDefault(x => x.RolID == akaMevcut.AkademisyenRolID).RolAdi;
                TempData["ProfilId"] = akaMevcut.AkademisyenId;
                ViewData["ProfilGosterAd"] = akaMevcut.AkademisyenAdi;
                ViewData["ProfilGosterSoyad"] = akaMevcut.AkademisyenSoyadi;
                ViewData["ProfilGosterMail"] = akaMevcut.AkademisyenMail;
                var biyografiGetir = db.Biografi.FirstOrDefault(x => x.BiografiSahibiID == akaMevcut.AkademisyenId);
                var detayGetir = db.SosyalMedya.FirstOrDefault(a => a.KullaniciId == akaMevcut.AkademisyenId);
                var akabolum = db.Bolumler.FirstOrDefault(x => x.BolumID == AkaData.AkademisyenBolumID).BolumAdi;

                if (akabolum != null)
                {
                    ViewData["EkBilgi"] = akabolum;

                }


                if (rolGetir != null)
                {
                    ViewData["ProfilGosterRol"] = rolGetir;
                }
                else
                {
                    return RedirectToAction("PageError", "Error");
                }

                if (biyografiGetir != null)
                {
                    TempData["BiyografiGoster"] = biyografiGetir.BiografiIcerik;

                }
                else
                {
                    TempData["BiyografiGoster"] = "Biyografi henüz eklenmemiş";

                }

                if (detayGetir != null)
                {
                    ViewData["ProfilGosterFace"] = detayGetir.SosyalMedyaFacebook;

                }
                else
                {
                    ViewData["ProfilGosterFace"] = " ";

                }
                if (detayGetir !=null)
                {
                    ViewData["ProfilGosterInsta"] = detayGetir.SosyalMedyaInstagram;

                }
                else
                {
                    ViewData["ProfilGosterInsta"] = " ";

                }
                if (detayGetir != null)
                {
                    ViewData["ProfilGosterLinkedin"] = detayGetir.SosyalMedyaLinkedin;

                }
                else
                {
                    ViewData["ProfilGosterLinkedin"] = " ";

                }
                if (detayGetir != null)
                {
                    ViewData["ProfilGosterDiger"] = detayGetir.DigerSite;

                }
                else
                {
                    ViewData["ProfilGosterDiger"] = "Eklenmemiş";

                }



            }
            else
            {
                var profilResim = db.ProfilResimleri.FirstOrDefault(a => a.ProfilResimID == ogrData.ProfilResimId).ProfilResmiYolu;
                ViewData["ProfilGosterResim"] = profilResim;
                var akaMevcut = db.Ogrenci.Find(ogrData.OgrenciNo);
                var rolGetir = db.Rol.FirstOrDefault(x => x.RolID == akaMevcut.OgrenciRolID).RolAdi;
                TempData["ProfilId"] = akaMevcut.OgrenciNo;
                ViewData["ProfilGosterAd"] = akaMevcut.OgrenciAdi;
                ViewData["ProfilGosterSoyad"] = akaMevcut.OgrenciSoyadi;
                ViewData["ProfilGosterMail"] = akaMevcut.OgrenciMail;
                var biyografiGetir = db.Biografi.FirstOrDefault(x => x.BiografiSahibiID == akaMevcut.OgrenciNo);
                var detayGetir = db.SosyalMedya.FirstOrDefault(a => a.KullaniciId == akaMevcut.OgrenciNo);
                var akabolum = db.Bolumler.FirstOrDefault(x => x.BolumID == ogrData.OgrenciBolumID).BolumAdi;
                var ogrenciSinif = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == ogrData.OgrenciNo).OgrenciSinif;

                if (ogrData.OgrenciRolID == 7)
                {
                    var firmadata = db.KisiselDetay.FirstOrDefault(x => x.KullaniciId == ogrData.OgrenciNo);
                    if(firmadata!=null)
                    {
                        var calistigiFirma = firmadata.CalistigiFirma;
                        var FirmaSehir = db.KisiselDetay.FirstOrDefault(x => x.KullaniciId == ogrData.OgrenciNo).CalistigiFirmaninIli;
                        var pozisyonu = db.KisiselDetay.FirstOrDefault(x => x.KullaniciId == ogrData.OgrenciNo).Bolumu;
                        var mezuniyetYili = db.KisiselDetay.FirstOrDefault(x => x.KullaniciId == ogrData.OgrenciNo).MezuniyetYili;
                        ViewData["MezuniyetYili"] = mezuniyetYili + "Yılında Mezun";
                        ViewData["calistigiYer"] = calistigiFirma;
                        ViewData["FirmaSehir"] = FirmaSehir;
                        ViewData["CalistigiPozisyon"] = pozisyonu;
                    }
                    else
                    {
                        ViewData["MezuniyetYili"] = " ";
                        ViewData["MezuniyetYili"] = " ";
                        ViewData["calistigiYer"] = " ";
                        ViewData["FirmaSehir"] = " ";
                        ViewData["CalistigiPozisyon"] = " ";
                    }
                }
                else
                {
                    ViewData["MezuniyetYili"] = " ";
                    ViewData["MezuniyetYili"] = " ";
                    ViewData["calistigiYer"] = " ";
                    ViewData["FirmaSehir"] = " ";
                    ViewData["CalistigiPozisyon"] = " ";

                }

                if (akabolum != null )
                {
                    string dene = "/ " + ogrenciSinif + " . Sınıf ";
                    if (ogrData.OgrenciRolID==7)
                    {
                        dene = "/ " + ogrenciSinif;
                    }
                    ViewData["EkBilgi"] = akabolum + dene;

                }

              


                if (rolGetir != null)
                {
                    ViewData["ProfilGosterRol"] = rolGetir;
                }
                else
                {
                    return RedirectToAction("PageError", "Error");
                }

                if (biyografiGetir != null)
                {
                    TempData["BiyografiGoster"] = biyografiGetir.BiografiIcerik;

                }
                else
                {
                    TempData["BiyografiGoster"] = "Biyografi henüz eklenmemiş";

                }

                if (detayGetir != null)
                {
                    ViewData["ProfilGosterFace"] = detayGetir.SosyalMedyaFacebook;

                }
                else
                {
                    ViewData["ProfilGosterFace"] = " ";

                }
                if (detayGetir != null)
                {
                    ViewData["ProfilGosterInsta"] = detayGetir.SosyalMedyaInstagram;

                }
                else
                {
                    ViewData["ProfilGosterInsta"] = " ";

                }
                if (detayGetir != null)
                {
                    ViewData["ProfilGosterLinkedin"] = detayGetir.SosyalMedyaLinkedin;

                }
                else
                {
                    ViewData["ProfilGosterLinkedin"] = " ";

                }
                if (detayGetir != null)
                {
                    ViewData["ProfilGosterDiger"] = detayGetir.DigerSite;

                }
                else
                {
                    ViewData["ProfilGosterDiger"] = "Eklenmemiş";

                }



            }
            return View();
        }


        public ActionResult Profilim()
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
        public ActionResult KendiPaylastiklarim()
        {

            int kulId = Convert.ToInt32(Session["KullaniciId"]);
            var veri = db.Duyurular.Where(x => x.DuyuruyuYapanId == kulId).Where(y => y.DuyuruDurum == true).OrderByDescending(m => m.DuyuruTarihi).ToList();
            return View(veri);
        }
        public ActionResult ProfilGosterPaylasilanlar()
        {
            int kulId = Convert.ToInt32(TempData["ProfilId"]);
            var veri = db.Duyurular.Where(x => x.DuyuruyuYapanId == kulId).Where(x => x.DuyuruDurum == true).OrderByDescending(m => m.DuyuruTarihi).ToList();
            return View(veri);
        }

        public ActionResult HesapAyarlari()
        {
            return View();
        }

        public ActionResult SifreDegistir()
        {

            if (Session["rol"] != null)
            {
                int id = Convert.ToInt32(Session["KullaniciId"]);

                var AkaData = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == id);
                var ogrDatam = db.Ogrenci.FirstOrDefault(y => y.OgrenciNo == id);

                if (ogrDatam != null)
                {
                    string OgrenciSifre = MD5Sifrele(Request["EskiSifre"].ToString());
                    string YeniSifre = MD5Sifrele(Request["YeniSifre"].ToString());
                    string YeniSifreTekrar = MD5Sifrele(Request["YeniSifreTekrar"].ToString());
                    var mevcut = db.Ogrenci.Find(ogrDatam.OgrenciNo);


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
                        db.SaveChanges();
                    }
                    return RedirectToAction("CikisYap", "OgrenciGiris", db.Ogrenci);
                }
                else
                {
                    string AkdemisyenSifre = Request["EskiSifre"].ToString();
                    string YeniSifre = Request["YeniSifre"].ToString();
                    string YeniSifreTekrar = Request["YeniSifreTekrar"].ToString();
                    var mevcut = db.Akademisyen.Find(AkaData.AkademisyenId);
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
                        db.SaveChanges();
                    }


                    return RedirectToAction("CikisYap", "OgrenciGiris", db.Ogrenci);
                }
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
        }

        public ActionResult HesapGuncelle()
        {
            if (Session["rol"] != null)
            {
                Biografi bf = new Biografi();
                int id = Convert.ToInt32(Session["KullaniciId"]);

                var AkaData = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == id);
                var ogrDatam = db.Ogrenci.FirstOrDefault(y => y.OgrenciNo == id);
                var akaBio = db.Biografi.FirstOrDefault(a => a.BiografiSahibiID == id);
                var ogrBio = db.Biografi.FirstOrDefault(b => b.BiografiSahibiID == id);

                if (ogrDatam != null)
                {
                    string adi = Request["adi"].ToString();
                    string soyadi = Request["soyadi"].ToString();
                    string mail = Request["email"].ToString();
                    string biyografi = Request["biyografi"].ToString();
                    var mevcut = db.Ogrenci.Find(ogrDatam.OgrenciNo);



                    if (adi == null || soyadi == null || mail == null || biyografi == null)
                    {
                        TempData["Message"] = "Bilgileri eksiksiz giriniz";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {

                        mevcut.OgrenciAdi = adi;
                        mevcut.OgrenciSoyadi = soyadi;
                        mevcut.OgrenciMail = mail;
                        if (ogrBio != null)
                        {
                            var mevcutBio = db.Biografi.Find(ogrBio.BiografiSahibiID);
                            mevcutBio.BiografiIcerik = biyografi;
                        }
                        else if (akaBio != null)
                        {
                            var mevcutBioAka = db.Biografi.Find(akaBio.BiografiID);
                            mevcutBioAka.BiografiIcerik = biyografi;

                        }
                        else
                        {
                            bf.BiografiIcerik = biyografi;
                            db.Biografi.Add(bf);
                            db.SaveChanges();
                        }
                        bf.BiografiSahibiID = id;
                        db.SaveChanges();
                    }
                    return RedirectToAction("CikisYap", "OgrenciGiris", db.Ogrenci);
                }
                else
                {
                    string AkdemisyenSifre = Request["EskiSifre"].ToString();
                    string YeniSifre = Request["YeniSifre"].ToString();
                    string YeniSifreTekrar = Request["YeniSifreTekrar"].ToString();
                    var mevcut = db.Akademisyen.Find(AkaData.AkademisyenId);
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
                        db.SaveChanges();
                    }


                    return RedirectToAction("CikisYap", "OgrenciGiris", db.Ogrenci);
                }
            }
            else
            {
                return RedirectToAction("CikisYap", "OgrenciGiris");
            }
        }

        public ActionResult ProfilResmiGuncelle(HttpPostedFileBase file)
        {
            string kullaniciId = Session["KullaniciId"].ToString();
            int Id = Convert.ToInt32(kullaniciId);
            Ogrenci ogr = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == Id);
            ProfilResimleri rsm = db.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == ogr.ProfilResimId);
            Image img = Image.FromStream(file.InputStream);
            img.Save(Server.MapPath("/Theme/KullaniciResimleri/" + file.FileName));
            rsm.ProfilResmiYolu = "/Theme/KullaniciResimleri/" + file.FileName;
            Session["ProfilResim"] = rsm.ProfilResmiYolu;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult BiyografiGoster()
        {
            int id = Convert.ToInt32(Session["KullaniciId"]);
            var listele = db.Biografi.FirstOrDefault(x => x.BiografiSahibiID == id);

            if (listele != null)
            {
                ViewData["Biyografim"] = listele.BiografiIcerik.ToString();

            }
            else
            {
                ViewData["Biyografim"] = "Bu kişi ne yazık ki, henüz kendisinden bahsetmemiş...";


            }
            return View();
        }
        public ActionResult ProfilGosterBiyografi()
        {
            int id = Convert.ToInt32(TempData["baskasininProfili"]);
            var listele = db.Biografi.FirstOrDefault(x => x.BiografiSahibiID == id);

            if (listele.BiografiIcerik != null)
            {
                TempData["Biyografim"] = listele.BiografiIcerik.ToString();


            }
            else
            {
                TempData["Biyografim"] = "Bu kişi ne yazık ki, henüz kendisinden bahsetmemiş...";


            }
            return View();
        }



    }
}