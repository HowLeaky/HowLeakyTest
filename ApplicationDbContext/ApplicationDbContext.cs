using HowLeakyModels._Custom;
using HowLeakyModels._Parameters;
using HowLeakyModels.Accounts;
using HowLeakyModels.AppData;
using HowLeakyModels.CMS;
using HowLeakyModels.Links;
using HowLeakyModels.News;
using HowLeakyModels.Version;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace HowLeakyModels.ApplicationDbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<CustomModel>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            MapToTable<NewsItem>(modelBuilder, "NewsItems");
            //MapToTable<GeoRegion>(modelBuilder, "Regions");

            modelBuilder.Entity<VegModel>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<SoilModel>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ParameterModel>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ParameterModelElement>().Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ParameterModel>().HasMany(x => x.Data).WithRequired(x => x.Parent).WillCascadeOnDelete(false);

            //modelBuilder.Entity<SoilModel>().HasMany(x => x.Regions).WithMany(x => x.Soils);
            modelBuilder.Entity<SoilModel>().HasMany(x => x.EditHistory).WithOptional(x => x.soil).WillCascadeOnDelete(false);
            modelBuilder.Entity<SoilModel>().HasMany(x => x.AdditionalPermissions);
            //modelBuilder.Entity<VegModel>().HasMany(x => x.Regions).WithMany(x => x.Vegetation);
            modelBuilder.Entity<VegModel>().HasMany(x => x.EditHistory).WithOptional(x => x.veg).WillCascadeOnDelete(false);
            modelBuilder.Entity<VegModel>().HasMany(x => x.AdditionalPermissions);

        }

        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<ClimateDataModel> ClimateDataModels { get; set; }
        //public virtual DbSet<GeoRegion> Regions { get; set; }

        // Website General
        public virtual DbSet<LinksRecord> LinksRecords { get; set; }
        public virtual DbSet<VersionRecord> VersionRecords { get; set; }

        public virtual DbSet<Cms> Cms { get; set; }

        public virtual DbSet<ParameterModelElement> ParameterModelElements { get; set; }



        private void MapToTable<TClassType>(DbModelBuilder modelBuilder, String tablename) where TClassType : class
        {
            modelBuilder.Entity<TClassType>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(tablename);
            });
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var model = entry.Entity as CustomUserRecord;
                if (model != null)
                {
                    var entity = model;
                    var username = GetUserName();
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.UtcNow;
                        entity.ModifiedDate = DateTime.UtcNow;
                        entity.CreatedBy = username;
                        entity.ModifiedBy = username;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.ModifiedDate = DateTime.UtcNow;
                        entity.ModifiedBy = username;
                    }
                }
            }
            return base.SaveChanges();
        }

        private static string GetUserName()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                return System.Web.HttpContext.Current.User.Identity.Name;
            return "unknown";
        }
    }
}