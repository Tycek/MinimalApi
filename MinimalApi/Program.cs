using MinimalApi;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/pickup", (SetPickupModel data) => Signal.SendPickup(data)).WithTags("Signal");
app.MapGet("/pickup", Signal.GetPickup).WithTags("Signal");
app.MapPost("/serialPickup", (SetPickupModel data) => SerialSignal.SendPickup(data)).WithTags("SerialSignal");
app.MapGet("/serialPickup", SerialSignal.GetPickup).WithTags("SerialSignal");

app.Run();

