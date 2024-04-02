using API.Helpers;
using API.Helpers.DI;
using Core.Helpers;
using System.Text.Json.Serialization;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

//DI
builder.Services.AddApplicationDI(builder.Configuration);

builder.Services.AddModelHelpersServices(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//var accountSid = builder.Configuration["Twilio:AccountSID"];
//var authToken = builder.Configuration["Twilio:AuthToken"];
//TwilioClient.Init(accountSid, authToken);

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
