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

app.MapPost("/pickup", (SetPickupModel data) => Signal.SendPickup(data));
app.MapGet("/pickup", Signal.GetPickup);

app.Run();

