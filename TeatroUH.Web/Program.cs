using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;

using TeatroUH.Infrastructure.Data;
using TeatroUH.Application.Interfaces;
using TeatroUH.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// FILE UPLOAD FIX (10 MB)
// ===============================
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
});

// ===============================
// MVC
// ===============================
builder.Services.AddControllersWithViews();

// ===============================
// DbContext
// ===============================
builder.Services.AddDbContext<TeatroUHDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ===============================
// Session
// ===============================
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ===============================
// Dependency Injection (SERVICES)
// ===============================
builder.Services.AddScoped<IPlayService, PlayService>();
builder.Services.AddScoped<IShowtimeService, ShowtimeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<INewsService, NewsService>();

var app = builder.Build();

// ===============================
// Middleware
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Archivos estáticos normales (wwwroot)
app.UseStaticFiles();

// ✅ Archivos estáticos EXTRA: /uploads (FUERA del proyecto)
var uploadsRoot = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "TeatroUH",
    "uploads"
);

Directory.CreateDirectory(uploadsRoot);

// ✅ test para verificar que sí crea/escribe en la carpeta
File.WriteAllText(
    Path.Combine(uploadsRoot, "test.txt"),
    $"Uploads OK - {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsRoot),
    RequestPath = "/uploads"
});

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// ===============================
// Routing
// ===============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
