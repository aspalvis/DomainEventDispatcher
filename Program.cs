using DomainEventDispatcher.Abstractions;
using DomainEventDispatcher.Data;
using DomainEventDispatcher.Data.Interceptors;
using DomainEventDispatcher.Domain.PersonAggregate;
using DomainEventDispatcher.EventHandlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

builder.Services.AddScoped<IDomainEventHandler<PersonCreatedDomainEvent>, PersonCreatedDomainEventHandler>();

builder.Services.AddDbContext<AppDbContext>((sp, o) =>
{
    o.UseInMemoryDatabase("application_db");
    o.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
