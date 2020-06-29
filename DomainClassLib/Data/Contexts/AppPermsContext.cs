using CommonLib;
using DomainClassLib.Data.Entities.AppPerm;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DomainClassLib.Data.Contexts
{
    public class AppPermsContext : DbContext
    {
        
        public AppPermsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<ControllerAccess> ControllerAccess { get; set; }
        public DbSet<MethodAccess> MethodAccess { get; set; }
        public DbSet<TokenTbl> TokenTbl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var appPermsDB = Utility.AppConfiguration().GetSection("ConnectionStrings").GetSection("AppPermsDB").Value;

            optionsBuilder.UseSqlite(appPermsDB, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppPermission>(entity =>
            {
                entity.HasKey(e => new { e.AppID, e.AppServerIP });
            });
            modelBuilder.Entity<ControllerAccess>(entity =>
            {
                entity.HasKey(e => new { e.AppID, e.ControllerName });
            });
            modelBuilder.Entity<MethodAccess>(entity =>
            {
                entity.HasKey(e => new { e.AppID, e.ControllerName, e.MethodName });
            });
            modelBuilder.Entity<TokenTbl>(entity =>
            {
                entity.HasKey(e => new { e.Refresh });
            });
            base.OnModelCreating(modelBuilder);
        }


    }
}
