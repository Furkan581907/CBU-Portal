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
    
    public partial class Duyurular
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string DuyuruResmi { get; set; }
        public Nullable<System.DateTime> DuyuruTarihi { get; set; }
        public Nullable<int> DuyuruyuYapanId { get; set; }
        public string DuyuruTipi { get; set; }
        public Nullable<bool> DuyuruDurum { get; set; }
        public Nullable<bool> DuyuruYorumaAcikMi { get; set; }
    }
}