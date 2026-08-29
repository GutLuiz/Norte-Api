using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using UepaMed.Application.Interfaces.Arquivos;
using UepaMed.Application.Interfaces.Artigos;
using UepaMed.Application.Interfaces.Convites;
using UepaMed.Application.Interfaces.Revisoes;
using UepaMed.Application.Interfaces.Usuarios;
using UepaMed.Infrastructure.Data;
using UepaMed.Infrastructure.Importers;
using UepaMed.Infrastructure.Repositories.Arquivos;
using UepaMed.Infrastructure.Repositories.Artigos;
using UepaMed.Infrastructure.Repositories.Revisoes;
using UepaMed.Infrastructure.Repositories.Usuarios;
using UepaMed.Application.Interfaces.Votacoes;
using UepaMed.Infrastructure.Repositories.Votacoes;
using UepaMed.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RevisaoService>();
builder.Services.AddScoped<IRevisaoRepository, RevisaoRepository>();
builder.Services.AddScoped<IImportadorArtigos, NbibImportador>();
builder.Services.AddScoped<IImportadorArtigos, RisImportador>();
builder.Services.AddScoped<IArtigoRepository, ArtigoRepository>();
builder.Services.AddScoped<ImportacaoArtigosService>();
builder.Services.AddScoped< IArquivoImportacaoRepository,ArquivoImportacaoRepository>();
builder.Services.AddScoped<IRevisaoMembroRepository, RevisaoMembroRepository>();
builder.Services.AddScoped<IConviteRevisaoRepository,ConviteRevisaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ConviteRevisaoService>();
builder.Services.AddScoped<IVotacaoRepository,VotacaoRepository>();
builder.Services.AddScoped<VotacaoService>();

builder.Services.AddScoped<
    IDuplicidadeRepository,
    DuplicidadeRepository>();

builder.Services.AddScoped<DuplicidadeService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();