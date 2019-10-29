﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace Gamer.Models
{
    public class Context : DbContext
    {
        public Context() : base("Gamer")
        {
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Game> Games { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}