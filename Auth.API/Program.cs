using Auth.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Controller Desteği
builder.Services.AddControllers();

// 2) EF Core Ayarları (MySQL)
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    // "DefaultConnection" -> appsettings.json içindeki ConnectionStrings alanına bakar
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
});

// 3) JWT Ayarlarını appsettings.json'dan Okuma
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

// 4) Authentication & JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

// 5) Authorization
builder.Services.AddAuthorization();

// Uygulama oluşturuluyor
var app = builder.Build();

// 6) Middleware Pipeline
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 7) Controller'ları Yönlendirme
app.MapControllers();

// 8) Uygulamayı Çalıştır
app.Run();