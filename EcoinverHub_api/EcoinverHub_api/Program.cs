using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EcoinverHub_api.Data;
using EcoinverHub_api.Models;
using EcoinverHub_api.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(System.Net.IPAddress.Any, 443, listenOptions =>
//    {
//        listenOptions.UseHttps("/ssl/server.pfx", "temp123");
//    });
//});

// 1) Configurar DbContext con MySQL (Pomelo)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));

// 2) Registrar el esquema de autenticación JWT como predeterminado
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],

        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 3) Registrar Identity usando nuestras entidades personalizadas
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 4) Registrar nuestro servicio de generación/validación de JWT
builder.Services.AddScoped<JWTService>();

// 5) Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 6) Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Para desarrollo: permite cualquier origen
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 7) Middleware pipeline

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseStaticFiles();  // <- Aquí habilitas servir archivos estáticos desde wwwroot

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
