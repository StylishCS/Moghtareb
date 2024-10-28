using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Moghtareb;
using Moghtareb.Models;
using Moghtareb.Repositories;
using Moghtareb.Services;
using Stripe;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SK");
builder.Services.AddDbContext<MoghtarebDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CS")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IAdRepository, AdRepository>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IWishlistRepository, WishlistRepository>();
builder.Services.AddSingleton<IAdViewsDetailsRepository, AdViewsDetailsRepository>();
builder.Services.AddSingleton<IAdReportRepository, AdReportRepository>();
builder.Services.AddSingleton<IUserServices, UserService>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe: SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
