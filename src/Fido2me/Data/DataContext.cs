using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC;
using Fido2me.Data.OIDC.ciba;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fido2me.Data
{
    public class DataContext : DbContext, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Credential> Credentials { get; set; }

        public DbSet<Attestation> Attestations { get; set; }

        public DbSet<Assertion> Assertions { get; set; }

        // https://github.com/DuendeSoftware/IdentityServer/blob/main/migrations/IdentityServerDb/Migrations/ConfigurationDb.sql
        public DbSet<OidcClient> OidcClients { get; set; }

        public DbSet<OidcBasicClient> OidcBasicClients { get; set; }

        public DbSet<CibaLoginRequest> CibaLoginRequests { get; set; }



        private IWebHostEnvironment _env;


        public DataContext(DbContextOptions<DataContext> options, IWebHostEnvironment env)
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
            var hexStringConverter = new ValueConverter<byte[], string>(
                v => Convert.ToHexString(v),
                v => Convert.FromHexString(v));

            modelBuilder.Entity<Account>()
            .ToContainer("Accounts")
            .HasNoDiscriminator();

            modelBuilder.Entity<Credential>()
            .ToContainer("Credentials")
            .HasNoDiscriminator();

            modelBuilder.Entity<Credential>().Property(p => p.Id).HasConversion(hexStringConverter);
            modelBuilder.Entity<Credential>().Property(p => p.UserHandle).HasConversion(hexStringConverter);
            modelBuilder.Entity<Credential>().Property(p => p.PublicKey).HasConversion(hexStringConverter);

            modelBuilder.Entity<Attestation>()
            .ToContainer("Attestations")
            .HasNoDiscriminator();

            modelBuilder.Entity<Attestation>().Property(p => p.Id).HasConversion(hexStringConverter);
            modelBuilder.Entity<Attestation>().Property(p => p.RawId).HasConversion(hexStringConverter);
            modelBuilder.Entity<Attestation>().Property(p => p.AttestationObject).HasConversion(hexStringConverter);
            modelBuilder.Entity<Attestation>().Property(p => p.ClientDataJson).HasConversion(hexStringConverter);       


            modelBuilder.Entity<Assertion>()
            .ToContainer("Assertions")
            .HasNoDiscriminator();

            modelBuilder.Entity<OidcClient>()
            .ToContainer("OidcClients")
            .HasNoDiscriminator();

            modelBuilder.Entity<OidcBasicClient>()
            .ToContainer("OidcBasicClients")
            .HasNoDiscriminator();

            modelBuilder.Entity<OidcBasicClient>().Property(p => p.ClientCorsOrigins).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
            modelBuilder.Entity<OidcBasicClient>().Property(p => p.ClientGrantTypes).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
            modelBuilder.Entity<OidcBasicClient>().Property(p => p.ClientRedirectUris).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );
            modelBuilder.Entity<OidcBasicClient>().Property(p => p.ClientScopes).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );

            modelBuilder.Entity<CibaLoginRequest>()
            .ToContainer("CibaLoginRequests")
            .HasNoDiscriminator();

            modelBuilder.Entity<CibaLoginRequest>().Property(p => p.RequestedScopes).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );

            modelBuilder.Entity<CibaLoginRequest>().Property(p => p.AuthorizedScopes).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            );

            if (_env.EnvironmentName == "Development")
            {
                //modelBuilder.SeedDevData();                               
            }
        }
 

    }


}
