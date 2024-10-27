using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Infrastructure.EventPublishing;
using ParrotInc.SquawkService.Application.Services;
using ParrotInc.SquawkService.Domain.Specifications;
using Carter;
using ParrotInc.SquawkService.Domain.Services;
using ParrotInc.SquawkService.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using ParrotInc.SquawkService.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using ParrotInc.SquawkService.Domain.Interfaces.ParrotInc.SquawkService.Domain.Services;
using ParrotInc.SquawkService.Application.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Drawing;

namespace ParrotInc.SquawkService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register other services
            services.AddScoped<ISquawkRepository, SquawkRepository>();
            services.AddScoped<ISquawkDomainService, SquawkDomainService>();
            // Register individual specifications
            services.AddScoped<ISquawkSpecification, ContentSpecification>();
            services.AddScoped<ISquawkSpecification, ContentRestrictionSpecification>();
            services.AddScoped<ISquawkSpecification>(provider => new ContentLengthSpecification(400));
            // Register the composite specification
            services.AddScoped<CompositeSquawkSpecification>(provider =>
            {
                var compositeSpecification = new CompositeSquawkSpecification();
                // Add individual specifications to the composite
                var specifications = provider.GetServices<ISquawkSpecification>();
                foreach (var specification in specifications)
                {
                    compositeSpecification.AddSpecification(specification);
                }
                return compositeSpecification;
            });

            // Register InMemoryEventPublisher as a singleton
            services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

            var serviceProvider = services.BuildServiceProvider();
            var eventPublisher = serviceProvider.GetRequiredService<IEventPublisher>();
            eventPublisher.RegisterEventHandler(async (domainEvent) =>
            {
                if (domainEvent is SquawkCreatedEvent createdEvent)
                {
                    Console.WriteLine($"Event received: {domainEvent.GetType().Name}");
                    await Task.CompletedTask;
                }
            });

            // Policies
            var retryPolicy = PolicyProvider.GetRetryPolicy();
            var circuitBreakerPolicy = PolicyProvider.GetCircuitBreakerPolicy();

            // Register the UserService with policies
            services.AddHttpClient<IUserService, UserDomainService>()
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(circuitBreakerPolicy);

            //Rate Limiting
            services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = 1;
                options.Window = TimeSpan.FromSeconds(20);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 0;
            }));

            //Event Handler
            services.AddScoped<IEventDatabaseService, EventDatabaseService>();

            //Database
            services.AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase("SquawkEventDb"));
            services.AddSingleton<ICacheService, FakeRedis>();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            services.AddSingleton(LogManager.Instance);

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.AddCarter();
            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Squawk Service API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseRateLimiter();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Squawk Service API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors("AllowAllOrigins");

            // Register event handler in a scope
            using (var scope = serviceProvider.CreateScope())
            {
                var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
                var eventDatabaseService = scope.ServiceProvider.GetRequiredService<IEventDatabaseService>();

                eventPublisher.RegisterEventHandler(async (domainEvent) =>
                {
                    if (domainEvent is SquawkCreatedEvent createdEvent)
                    {
                        await eventDatabaseService.SaveEventAsync(createdEvent);
                        Console.WriteLine($"Event saved: {createdEvent.GetType().Name} for SquawkId {createdEvent.SquawkId}");
                    }
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCarter();
            });
        }
    }
}