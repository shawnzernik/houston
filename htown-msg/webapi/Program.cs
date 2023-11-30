using webapi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

var weatherEndpoint = new WeatherEndpoint(app);
var messageEndpoint = new MessageEndpoint(app);
var userEndpoint = new UserEndpoint(app);

app.Run();