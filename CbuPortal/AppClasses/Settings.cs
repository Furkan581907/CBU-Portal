using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CbuPortal.AppClasses
{
    public class Settings
    {
        public static Size ProfilResimBoyut
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["profilGenislik"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["profilYukseklik"]);
                return sonuc;

            }
        }
    }
}