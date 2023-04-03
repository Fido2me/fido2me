using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC.ConfigurationDb;
using Fido2me.Data.OIDC.PersistedGrantDb;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Data
{
    public class ApplicationDataContext : DbContext, IDataProtectionKeyContext
    {
        // // https://github.com/DuendeSoftware/IdentityServer/blob/main/migrations/IdentityServerDb/Migrations/ConfigurationDb.sql
        public DbSet<DataProtectionKey> DataProtectionKeys => null!;

        // Configuration dataset OIDC
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        public DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }
        public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
        public DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        public DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientProperty> ClientProperties { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        public DbSet<IdentityProvider> IdentityProviders { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<IdentityResourceClaim> IdentityResourceClaims { get; set; }
        public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

        // Persisted grant dataset OIDC
        public DbSet<DeviceCodeFlow> DeviceCodes { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<ServerSideSession> ServerSideSessions { get; set; }

        // FIDO2 dataset
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Assertion> Assertions { get; set; }
        public DbSet<Attestation> Attestations { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }

        private IWebHostEnvironment _env;
                
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options, IWebHostEnvironment env)
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
            modelBuilder.Entity<Attestation>()
            .Property(u => u.AttestionResult)
            .HasConversion<byte>();

            modelBuilder.Entity<Account>()
            .Property(u => u.AccountType)
            .HasConversion<byte>();


            if (_env.EnvironmentName == "Development")
            {
                //modelBuilder.SeedDevData();                               
            }
        }
    }
}
