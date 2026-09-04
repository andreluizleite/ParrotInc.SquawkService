using System.Threading.RateLimiting;
using ParrotInc.SquawkService.Api;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.Services;
using ParrotInc.SquawkService.Infrastructure.EventPublishing;
using ParrotInc.SquawkService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblyContaining<CreateSquawkCommand>());

// Singleton lifetimes are intentional: this portfolio sample uses in-memory state.
builder.Services.AddSingleton<ISquawkRepository, SquawkRepository>();
builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
builder.Services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<ISquawkDomainService, SquawkDomainService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("api", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRateLimiter();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health/live");
app.MapSquawkEndpoints();

app.Run();

public partial class Program;
