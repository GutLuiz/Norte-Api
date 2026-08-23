using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using UepaMed.Application.Interfaces;
using UepaMed.Application.Services;
using UepaMed.Infrastructure.Data;
using UepaMed.Infrastructure.Importers;
using UepaMed.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RevisaoService>();
builder.Services.AddScoped<IRevisaoRepository, RevisaoRepository>();
builder.Services.AddScoped<IImportadorArtigos, NbibImportador>();
builder.Services.AddScoped<IArtigoRepository, ArtigoRepository>();
builder.Services.AddScoped<ImportacaoArtigosService>();
builder.Services.AddScoped< IArquivoImportacaoRepository,ArquivoImportacaoRepository>();

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

//var caminho = @"C:\teste\pubmed-outcomesty-set.nbib";

//using var arquivo = File.OpenRead(caminho);

//var importador = new NbibImportador();

//var artigos = await importador.ImportarAsync(arquivo);

//foreach (var artigo in artigos)
//{
//    Console.WriteLine($"PMID: {artigo.PMID}");
//    Console.WriteLine($"Título: {artigo.Titulo}");
//    Console.WriteLine($"Resumo: {artigo.Resumo}");
//    Console.WriteLine($"Revista: {artigo.Revista}");
//    Console.WriteLine($"Autores: {artigo.Autores}");
//    Console.WriteLine($"DOI: {artigo.DOI}");
//    Console.WriteLine($"Ano: {artigo.AnoPublicacao}");
//    Console.WriteLine("-----------------------------------");
//}
app.Run();