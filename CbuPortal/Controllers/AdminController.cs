using CbuPortal.Models;
using CbuPortal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RolGorme = CbuPortal.Models.RolGorme;

namespace CbuPortal.Controllers
{
    public class AdminController : Controller
    {
        cbuTezEntities db = new cbuTezEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AdminMail"] != null)
            {
                return View();

            }
            else
                return RedirectToAction("Login", "Admin");

        }



        public string ToplamAkademisyen()
        {
            var data = db.Akademisyen.ToList();

            int sayi = data.Count();

            return sayi.ToString();
        }

        public string ToplamOgrenci()
        {

            var data = db.Ogrenci.Where(x=>x.OgrenciRolID==6).ToList();

            int sayi = data.Count();

            return sayi.ToString();
        }
        public string ToplamMezun()
        {
            var data = db.Ogrenci.Where(x => x.OgrenciRolID == 7).ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }
        public string ToplamOdev()
        {
            var data = db.Duyurular.Where(x => x.DuyuruTipi =="6").ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }
        public string ToplamIsDuyurusu()
        {
            var data = db.Duyurular.Where(x => x.DuyuruTipi == "8").ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }
        public string ToplamGonderi()
        {
            var data = db.Duyurular.ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }
        public string ToplamBolum()
        {
            var data = db.Bolumler.ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }
        public string ToplamYorum()
        {
            var data = db.Yorum.ToList();

            int sayi = data.Count();

            return sayi.ToString();

        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult GirisYap(Admin admin)
        {
            string sifre = MD5Sifrele(admin.AdminSifre);
            Admin login = db.Admin.FirstOrDefault(s => s.AdminMail == admin.AdminMail && s.AdminSifre == sifre);
            RolGorme data = new RolGorme();
            if (login != null)
            {
                RolGorme rolGormeData = new RolGorme
                {
                 AdminMail = admin.AdminMail  
                };
                data = rolGormeData;
                Session["AdminMail"] = rolGormeData.AdminMail;


                return RedirectToAction("Index", "Admin");

            }
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult LogOut()
        {
            return View();
        }

        public ActionResult AkademisyenListele()
        {
            if (Session["AdminMail"] != null)
            {
                List<AkademisyenListEntities> list = new List<AkademisyenListEntities>();
                List<Akademisyen> lst = db.Akademisyen.ToList();
                foreach (var item in lst)
                {
                    AkademisyenListEntities data = new AkademisyenListEntities();
                    data.AkademisyenAdi = item.AkademisyenAdi;
                    data.AkademisyenSoyad = item.AkademisyenSoyadi;
                    data.AkademisyenId = item.AkademisyenId;
                    data.AkademisyenMail = item.AkademisyenMail;
                    data.KayitTarihi = item.AkademisyenUyelikTarihi.Value;
                    string bolumAdi = db.Bolumler.FirstOrDefault(s => s.BolumID == item.AkademisyenBolumID).BolumAdi;
                    data.AkademisyenBolumAdi = bolumAdi;
                    data.AkademisyenBolumId = item.AkademisyenBolumID;
                    string rol = db.Rol.FirstOrDefault(s => s.RolID == item.AkademisyenRolID).RolAdi;
                    data.RolAdi = rol;
                    data.AkademisyenRolID = item.AkademisyenRolID;
                    string resimYolu = db.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == item.ProfilResimId).ProfilResmiYolu;
                    data.ResimYolu = resimYolu;
                    data.Aktiflik = item.Aktiflik;
                    data.KabulDurumu = item.KabulDurumu;
                    list.Add(data);


                }
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


        

        public ActionResult AkademisyenSil(int id)
        {
            if (Session["AdminMail"] != null)
            {
                Akademisyen aka = db.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
                aka.Aktiflik = false;
                db.SaveChanges();
                return RedirectToAction("AkademisyenListele", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


        public ActionResult AkademisyenDuzenle(Akademisyen aka, int id)
        {
            if (Session["AdminMail"] != null)
            {
                Akademisyen editaka = db.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
                editaka.AkademisyenBolumID = aka.AkademisyenBolumID;
                editaka.Aktiflik = aka.Aktiflik;
                editaka.KabulDurumu = aka.KabulDurumu;
                editaka.AkademisyenAdi = aka.AkademisyenAdi;
                editaka.AkademisyenSoyadi = aka.AkademisyenSoyadi;
                editaka.AkademisyenMail = aka.AkademisyenMail;
                db.SaveChanges();
                return RedirectToAction("AkademisyenListele", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }




        public ActionResult AkademisyenGoruntule(int? id)
        {
            if (Session["AdminMail"] != null)
            {
                Akademisyen aka = db.Akademisyen.FirstOrDefault(s => s.AkademisyenId == id);
                AkademisyenListEntities data = new AkademisyenListEntities();
                data.AkademisyenId = aka.AkademisyenId;
                data.AkademisyenAdi = aka.AkademisyenAdi;
                data.AkademisyenBolumAdi = db.Bolumler.FirstOrDefault(s => s.BolumID == aka.AkademisyenBolumID).BolumAdi;
                data.AkademisyenMail = aka.AkademisyenMail;
                data.AkademisyenSoyad = aka.AkademisyenSoyadi;
                data.Aktiflik = aka.Aktiflik;
                data.KabulDurumu = aka.KabulDurumu;
                data.AkademisyenBolumId = aka.AkademisyenBolumID;

                return View(data);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
            
        }

        public ActionResult OgrenciListele()
        {
            if (Session["AdminMail"] != null)
            {
                List<OgrenciListEntities> list = new List<OgrenciListEntities>();
                List<Ogrenci> lst = db.Ogrenci.ToList();
                foreach (var item in lst)
                {
                    OgrenciListEntities data = new OgrenciListEntities();
                    data.OgrenciAdi = item.OgrenciAdi;
                    data.OgrenciSoyadi = item.OgrenciSoyadi;
                    data.OgrenciNo = item.OgrenciNo;
                    data.OgrenciMail = item.OgrenciMail;
                    data.OgrenciUyelikTarihi = item.OgrenciUyelikTarihi;
                    string bolumAdi = db.Bolumler.FirstOrDefault(s => s.BolumID == item.OgrenciBolumID).BolumAdi;
                    data.BolumAdi = bolumAdi;
                    data.OgrenciBolumID = item.OgrenciBolumID;
                    string rol = db.Rol.FirstOrDefault(s => s.RolID == item.OgrenciRolID).RolAdi;
                    data.RolAdi = rol;
                    data.OgrenciRolID = item.OgrenciRolID;
                    string resimYolu = db.ProfilResimleri.FirstOrDefault(s => s.ProfilResimID == item.ProfilResimId).ProfilResmiYolu;
                    data.ResimYolu = resimYolu;
                    data.Aktiflik = item.Aktiflik;
                    list.Add(data);


                }
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult OgrenciDuzenle(Ogrenci ogr,int id)
        {
            if (Session["AdminMail"] != null)
            {
                Ogrenci editaka = db.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
                editaka.OgrenciBolumID = ogr.OgrenciBolumID;
                editaka.Aktiflik = ogr.Aktiflik;
                editaka.OgrenciAdi = ogr.OgrenciAdi;
                editaka.OgrenciSoyadi = ogr.OgrenciSoyadi;
                editaka.OgrenciMail = ogr.OgrenciMail;
                editaka.OgrenciNo = ogr.OgrenciNo;
                editaka.OgrenciTelefon = ogr.OgrenciTelefon;
                db.SaveChanges();
                return RedirectToAction("OgrenciListele", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult OgrenciSil(int id)
        {
            if (Session["AdminMail"] != null)
            {
                Ogrenci ogr = db.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
                ogr.Aktiflik = false;
                db.SaveChanges();
                return RedirectToAction("OgrenciListele", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult OgrenciGoruntule(int id)
        {
            if (Session["AdminMail"] != null)
            {
                Ogrenci aka = db.Ogrenci.FirstOrDefault(s => s.OgrenciNo == id);
                OgrenciListEntities data = new OgrenciListEntities();
                data.OgrenciNo = aka.OgrenciNo;
                data.OgrenciAdi = aka.OgrenciAdi;
                data.BolumAdi = db.Bolumler.FirstOrDefault(s => s.BolumID == aka.OgrenciBolumID).BolumAdi;
                data.OgrenciMail = aka.OgrenciMail;
                data.OgrenciSoyadi = aka.OgrenciSoyadi;
                data.Aktiflik = aka.Aktiflik;
                data.OgrenciTelefon = aka.OgrenciTelefon;
                data.Aktiflik = aka.Aktiflik;
                data.OgrenciRolID = aka.OgrenciRolID;
                data.RolAdi = db.Rol.FirstOrDefault(s => s.RolID == aka.OgrenciRolID).RolAdi;


                return View(data);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult DosyaGoruntule()
        {
            if (Session["AdminMail"] != null)
            {
                List<Dosyalar> dosya = db.Dosyalar.ToList();
                return View(dosya);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult MesajGoruntule()
        {
            if (Session["AdminMail"] != null)
            {
                List<MesajListEntities> list = new List<MesajListEntities>();
                List<Mesajlar> lst = db.Mesajlar.ToList();
                foreach (var item in lst)
                {
                    MesajListEntities data = new MesajListEntities();
                    data.AliciId = item.AliciId;
                    if (data.AliciId.ToString().Length == 9)
                    {
                        string ad = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == data.AliciId).OgrenciAdi;
                        string soyad = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == data.AliciId).OgrenciSoyadi;
                        data.AliciAdi = ad + " " + soyad;
                    }
                    else
                    {
                        string ad = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == data.AliciId).AkademisyenAdi;
                        string soyad = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == data.AliciId).AkademisyenSoyadi;
                        data.AliciAdi = ad + " " + soyad;
                    }
                    data.GonderenId = item.GonderenId;
                    if (data.GonderenId.ToString().Length == 9)
                    {
                        string ad = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == data.GonderenId).OgrenciAdi;
                        string soyad = db.Ogrenci.FirstOrDefault(x => x.OgrenciNo == data.GonderenId).OgrenciSoyadi;
                        data.GonderenAdi = ad + " " + soyad;
                    }
                    else
                    {
                        string ad = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == data.GonderenId).AkademisyenAdi;
                        string soyad = db.Akademisyen.FirstOrDefault(x => x.AkademisyenId == data.GonderenId).AkademisyenSoyadi;
                        data.GonderenAdi = ad + " " + soyad;
                    }
                    data.Mesaj = item.Mesaj;
                    data.MesajTarihi = item.MesajTarihi;
                    list.Add(data);


                }
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
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


        public ActionResult AdminProfilDuzenle(Admin admin,int id)
        {
            if (Session["AdminMail"] != null)
            {
                var adm = db.Admin.FirstOrDefault(s => s.Id == id);
                adm.AdminMail = admin.AdminMail;
                adm.AdminSifre = MD5Sifrele(admin.AdminSifre);
                db.SaveChanges();
                return RedirectToAction("AdminListele", "Admin");
            }
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult AdminListele()
        {
            if (Session["AdminMail"] != null)
            {
                List<Admin> admin = db.Admin.ToList();
                return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AdminGoruntule(int id)
        {
            if (Session["AdminMail"] != null)
            {
                Admin admin = db.Admin.FirstOrDefault(s => s.Id == id);
                return View(admin);
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
        }

        public ActionResult YeniAdminEkle(Admin admin)
        {
            if (Session["AdminMail"] != null)
            {
                Admin yeni = new Admin();
                yeni.AdminMail = admin.AdminMail;
                yeni.AdminSifre = MD5Sifrele(admin.AdminSifre);
                db.Admin.Add(yeni);
                db.SaveChanges();
                return RedirectToAction("AdminListele", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AdminEkle()
        {
            if (Session["AdminMail"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


    }
}