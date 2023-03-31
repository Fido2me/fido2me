using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC.ConfigurationDb;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Data
{
    public class ApplicationDataContext : DbContext, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys => null!;


        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }


        private IWebHostEnvironment _env;
                
        public ApplicationDataContext(DbContextOptions<DataContext> options, IWebHostEnvironment env)
            : base(options)
        {
            _env = env;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_env.EnvironmentName == "Development")
            {
                //modelBuilder.SeedDevData();                               
            }
        }
    }
}
