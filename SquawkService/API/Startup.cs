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
using Microsoft.Extensions.DependencyInjection;

namespace ParrotInc.SquawkService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            // Register other services
            services.AddScoped<ISquawkRepository, SquawkRepository>();
            services.AddScoped<ISquawkDomainService, SquawkDomainService>();
            services.AddScoped<ISquawkAppService, SquawkAppService>();
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

            //Event Handler
            services.AddScoped<IEventDatabaseService, EventDatabaseService>();

            //Database
            services.AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase("SquawkEventDb"));


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