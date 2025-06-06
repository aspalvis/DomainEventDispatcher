using System.Reflection;
using DomainEventDispatcher.Data;
using DomainEventDispatcher.Data.Interceptors;
using DomainEventDispatcher.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Idea
builder.Services.AddDomainEventHandlers(Assembly.GetExecutingAssembly());

// Data
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp, o) =>
{
    o.UseInMemoryDatabase("application_db");
    o.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
});

// Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
