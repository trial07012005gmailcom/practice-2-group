using PatientManager.Managers;
using PatientManager.Services;
using Serilog;
using Services.ExternalServices;
using System.Security.Cryptography.Xml;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog(); 

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GiftManager>(); //Scoped porque usa servicios como ElectronicStoreService y IConfiguration,
                                           //no necesita estar en la memoria todo el tiempo y no debe recrearse en cada llamada 
builder.Services.AddScoped<PatientService>(); //Usa archivo físico y se llama varias veces por request

builder.Services.AddScoped<ElectronicStoreService>(); //Hace llamadas HTTP externas, entonces no es completamente liviano (Transient)
                                                      //Y tienen una dependencia inyectada, entonces debe mantenerse estable durante solo un request 
builder.Services.AddHttpClient<PatientCodeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
