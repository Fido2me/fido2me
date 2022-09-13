using Azure.Identity;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Validation;
using Fido2me;
using Fido2me.Data;
using Fido2me.Duende;
using Fido2me.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SendGrid.Extensions.DependencyInjection;

// useful link: https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;

config.AddAzureKeyVault(
    new Uri($"https://{config["vaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential(new DefaultAzureCredentialOptions
    {
        ExcludeEnvironmentCredential = true,
        ExcludeInteractiveBrowserCredential = true,
        ExcludeAzurePowerShellCredential = true,
        ExcludeSharedTokenCacheCredential = true,
        ExcludeVisualStudioCodeCredential = true,
        ExcludeVisualStudioCredential = true,
        ExcludeAzureCliCredential = builder.Environment.IsProduction(),
        ExcludeManagedIdentityCredential = !builder.Environment.IsProduction(),
    }));

StartupConfigurationHelper.ConfigureApplicationInsights(services, config["tracing-key"]);

services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    
    options.Conventions.AllowAnonymousToFolder("/auth");
    options.Conventions.AllowAnonymousToFolder("/General");
});

services.AddControllers();

services.AddDbContext<DataContext>(options =>
{
    options.UseCosmos(
        config["db-connectionstring"], config["db-name"],
        opt =>
        {
            opt.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
        });

});

// https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-6.0.
// use db context for now
services.AddDataProtection()
    .DisableAutomaticKeyGeneration() // TODO: manual rotation to avoid inserting duplicate records to DB?
    .PersistKeysToDbContext<DataContext>();


// Use the in-memory implementation of IDistributedCache.
// services.AddDistributedMemoryCache();

builder.Services.AddDataProtection();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.AccessDeniedPath = new PathString("/General/AccessDenied");
    options.Cookie.Name = "idsrv_identity"; // idsrv.session
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.LoginPath = new PathString("/auth/login");
    options.Cookie.SameSite = SameSiteMode.Strict;
});
//.AddOpenIdConnect();

builder.Services.ConfigureApplicationCookie(options =>
 {
     options.AccessDeniedPath = new PathString("/General/AccessDenied");
     options.Cookie.Name = "Cookie";
     options.Cookie.HttpOnly = true;
     options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
     options.LoginPath = new PathString("/auth/login");
     options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
     options.SlidingExpiration = true;
     options.Cookie.SameSite = SameSiteMode.Strict;
 });

/*
services.AddSession(options =>
{
    // Set a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    // Strict SameSite mode is required because the default mode used
    // by ASP.NET Core 3 isn't understood by the Conformance Tool
    // and breaks conformance testing
    options.Cookie.SameSite = SameSiteMode.Unspecified;
});
*/

services.AddFido2(options =>
{
    options.ServerDomain = config["fido2:serverDomain"];
    options.ServerName = config["fido2:serverName"];
    options.Origins = new HashSet<string> { config["fido2:origin"]  }; 
    options.TimestampDriftTolerance = config.GetValue<int>("fido2:timestampDriftTolerance");
    options.MDSCacheDirPath = config["fido2:MDSCacheDirPath"];
})
.AddCachedMetadataService(config =>
{
    config.AddFidoMetadataRepository();
});

services.AddIdentityServer(options =>
{
    options.Authentication.CookieLifetime = TimeSpan.FromMinutes(60);
    options.Authentication.CookieSlidingExpiration = true;
    options.IssuerUri = config["oidc:issuerUri"];
   
})    
    .AddClientStore<ClientStore>()
    .AddCorsPolicyService<CorsPolicyService>()
    .AddPersistedGrantStore<PersistedGrantStore>()
    .AddResourceStore<ResourceStore>();

services.AddSendGrid(options =>
{
    options.ApiKey = config["sendgrid-apikey"];
});    

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IFidoService, FidoService>();
builder.Services.AddTransient<IFidoRegistrationService, FidoRegistrationService>();
builder.Services.AddTransient<IFidoLoginService, FidoLoginService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IOidcBasicClientService, OidcBasicClientService>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IBackChannelAuthenticationRequestStore, BackChannelAuthenticationRequestStore>();
builder.Services.AddTransient<IBackchannelAuthenticationUserValidator, BackchannelAuthenticationUserValidator>();
builder.Services.AddTransient<IBackchannelAuthenticationUserNotificationService, BackchannelAuthenticationUserNotificationService>();
builder.Services.AddSingleton<IDiscoveryService, DiscoveryService>();
builder.Services.AddTransient<ICibaService, CibaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    config.AddApplicationInsightsSettings(developerMode: true);

    using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    using (var context = scope.ServiceProvider.GetService<DataContext>())
    {
        //context?.Database.EnsureDeleted();                    
        context?.Database.EnsureCreated();
    }
}
else
{
    app.UseExceptionHandler("/general/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseForwardedHeaders();
    app.UseHsts();

    using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    using (var context = scope.ServiceProvider.GetService<DataContext>())
    {
        //context.Database.EnsureDeleted();                    
        context?.Database.EnsureCreated();
    }

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
    endpoints.MapRazorPages().RequireAuthorization();
});
          

app.Run();
