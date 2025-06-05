using DomainEventDispatcher.Abstractions;
using DomainEventDispatcher.Data;
using DomainEventDispatcher.Data.Interceptors;
using DomainEventDispatcher.Domain.PersonAggregate;
using DomainEventDispatcher.Features.Person.Create;
using DomainEventDispatcher.Features.Person.EventHandlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CreatePersonCommand>();

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

app.Run();
