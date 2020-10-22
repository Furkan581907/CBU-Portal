using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CbuPortal.App_Start
{
    public class BundleConfig
    {
        

        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Theme/js").Include(
                                  "~/Theme/js/bootstrap.min.js",
                                    "~/Theme/js/jquery-3.2.1.slim.min.js.js",
                                      "~/Theme/js/jquery-3.1.1.min.js",
                                        "~/Theme/js/popper.js"
                        ));
            bundles.Add(new ScriptBundle("~/Theme/css/css").Include(
                "~/Theme/css/anasayfa.css",
                 "~/Theme/css/arkadaslar.css",
                  "~/Theme/css/bootstrap.css",
                   "~/Theme/css/bootstrap.min.css",
                    "~/Theme/css/giris.css",
                     "~/Theme/css/isilanlari.css",
                      "~/Theme/css/profil.css",
                      "~/Theme/css/all.css",
                      "~/Theme/css/sweetalert2.css",
                      "~/Theme/css/sweetalert2.min.css"

                ));

            bundles.Add(new ScriptBundle("~/Theme/js/sweetalert/js").Include(
                                "~/Theme/js/sweetalert/sweetalert2.js",
                                "~/Theme/js/sweetalert/sweetalert2.all.min.js",
                                "~/Theme/js/sweetalert/sweetalert2.all.js",
                                "~/Theme/js/sweetalert/sweetalert2.min.js"

));
            BundleTable.EnableOptimizations = true;
        }
    }  }