//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CbuPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dosyalar
    {
        public int DosyaID { get; set; }
        public Nullable<int> DosyaDuyuruID { get; set; }
        public string DosyaYolu { get; set; }
        public Nullable<int> DosyaSahibiID { get; set; }
        public Nullable<System.DateTime> DosyaYuklemeTarihi { get; set; }
        public string DosyaAciklamasi { get; set; }
    }
}
