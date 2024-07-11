using ChatAppAPI.Configurators;
using ChatAppAPI.Context;
using ChatAppAPI.Hubs;
using ChatAppAPI.Servisler.Kullanicilar;
using ChatAppAPI.Servisler.Mesajlar;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using ChatAppAPI.Servisler.OturumYonetimi;
using ChatAppAPI.Servisler.OturumYonetimi.DTOs;
using ChatAppAPI.Servisler.OturumYonetimi.JWT;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


SwaggerConfigurator.ConfigureSwaggerGen(builder.Services);

builder.Services.AddDbContext<ChatAppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSignalR();
builder.Services.AddScoped<IMesajServisi, MesajServisi>();
builder.Services.AddScoped<IOturumYonetimi, OturumYonetimi>();
builder.Services.AddScoped<IJwtServisi, JwtServisi>();
builder.Services.AddTransient<IKullaniciServisi, KullaniciServisi>();
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformerGelistirici>();


builder.Services.AddScoped<IValidator<MesajGonderDTO>, MesajGonderDTOValidator>();
builder.Services.AddScoped<IValidator<MesajlariGorulduYapDTO>, MesajlariGorulduYapDTOValidator>();

builder.Services.AddScoped<IValidator<KullaniciKayitDto>, KullaniciKayitDtoValidator>();
builder.Services.AddScoped<IValidator<KullaniciGirisDto>, KullaniciGirisDtoValidator>();
builder.Services.AddScoped<IValidator<KullaniciAdiIleGirisYapDTO>, KullaniciAdiIleGirisYapDTOValidator>();




builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")!)
    .AddSignalRHub(builder.Configuration["SignalR:HubUrl"]!);

builder.Services.AddResponseCaching(_ =>
{
    _.MaximumBodySize = 250;//Response Body�ler i�in ge�erli maksimum boyut. Varsay�lan olarak 64 MB�t�r.
    _.SizeLimit = 250;//Response Cache�in maksimum ne kadar boyutta tutulaca��n� belirtiriz. Varsay�lan olarak 100 MB de�erine sahiptir.
    _.UseCaseSensitivePaths = false;//Path de�erinin b�y�k ya da k���k harf duyarl���nda olup olmamas�n� belirler.
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? string.Empty)
                    ),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        expires != null ? expires > DateTime.UtcNow : false,

                };
            });

builder.Services.AddAuthorization();

builder.Services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior


builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseResponseCaching();

app.UseHttpsRedirection();
//app.UseRouting();

app.UseCors();
app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/mesaj");

app.Run();
