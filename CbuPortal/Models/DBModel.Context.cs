﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class cbuTezEntities : DbContext
    {
        public cbuTezEntities()
            : base("name=cbuTezEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Akademisyen> Akademisyen { get; set; }
        public virtual DbSet<Biografi> Biografi { get; set; }
        public virtual DbSet<Bolumler> Bolumler { get; set; }
        public virtual DbSet<DosyaGonderimleri> DosyaGonderimleri { get; set; }
        public virtual DbSet<Dosyalar> Dosyalar { get; set; }
        public virtual DbSet<Duyurular> Duyurular { get; set; }
        public virtual DbSet<DuyuruTipleri> DuyuruTipleri { get; set; }
        public virtual DbSet<Favoriler> Favoriler { get; set; }
        public virtual DbSet<IsIlani> IsIlani { get; set; }
        public virtual DbSet<KisiselDetay> KisiselDetay { get; set; }
        public virtual DbSet<KullaniciRol> KullaniciRol { get; set; }
        public virtual DbSet<Mesajlar> Mesajlar { get; set; }
        public virtual DbSet<Ogrenci> Ogrenci { get; set; }
        public virtual DbSet<ProfilResimleri> ProfilResimleri { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<SosyalMedya> SosyalMedya { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Yorum> Yorum { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
    }
}
