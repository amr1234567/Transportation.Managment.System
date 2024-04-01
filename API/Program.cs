using API.Helpers.DI;
using Core.Helpers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//DI
builder.Services.AddApplicationDI(builder.Configuration);

builder.Services.Configure<JwtHelper>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<MailConfigurations>(builder.Configuration.GetSection("EmailConfigration"));


builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
