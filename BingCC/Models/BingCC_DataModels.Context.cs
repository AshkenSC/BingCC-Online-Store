﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BingCC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BingCC_Entities : DbContext
    {
        public BingCC_Entities()
            : base("name=BingCC_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetOrderProducts> AspNetOrderProducts { get; set; }
        public virtual DbSet<AspNetOrders> AspNetOrders { get; set; }
        public virtual DbSet<AspNetProducts> AspNetProducts { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetCartProducts> AspNetCartProducts { get; set; }
    }
}
