using MinimalApi;
using System.Security.Cryptography.Xml;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", LocalFunction);
app.MapPost("/Signal", (SendDataModel data) => SendSignal(data));

app.Run();

string LocalFunction() => "This is local function";

void SendSignal(SendDataModel data) {
    Signal.Send(JsonSerializer.Serialize(data));
}