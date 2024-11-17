using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;


var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json");


// Add Ocelot services
builder.Services.AddOcelot().AddPolly();

var app = builder.Build();

// Use Ocelot middleware
app.UseOcelot().Wait();

app.Run();