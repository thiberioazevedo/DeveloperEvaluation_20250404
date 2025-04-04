using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Infrastructure.Persistence;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName);
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.Infrastructure")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            var activator = new BuiltinHandlerActivator();

            activator.Register(() => new SaleCreatedEventHandler());
            activator.Register(() => new SaleUpdatedEventHandler());
            activator.Register(() => new SaleCancelledEventHandler());
            activator.Register(() => new SaleItemDeletedEventHandler());
            activator.Register(() => new UserCreatedEventHandler());

            builder.Services.AddRebus(
                rebus => Configure
                    .With(activator)
                    .Transport(t => t.UseRabbitMq(builder.Configuration.GetConnectionString("RabbitMqConnection"), inputQueueName: "saleQueue"))
                    .Routing(r => r.TypeBased()
                    .Map<SaleCreatedEvent>("saleQueue")
                    .Map<SaleModifiedEvent>("saleQueue")
                    .Map<SaleCancelledEvent>("saleQueue")
                    .Map<SaleItemDeletedEvent>("saleQueue")
                    .Map<UserCreatedEvent>("saleQueue"))  
                    .Sagas(s =>
                        s.StoreInPostgres(
                            builder.Configuration.GetConnectionString("SagasConnection"),
                            dataTableName: "Sagas",
                            indexTableName: "SagaIndexes"))
                    .Timeouts(t =>
                        t.StoreInPostgres(
                            builder.Configuration.GetConnectionString("SagasConnection"),
                            tableName: "Timeouts")));

            builder.Services.AutoRegisterHandlersFromAssemblyOf<ApplicationLayer>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();

            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               //.WithOrigins("http://localhost:4200")
               //.AllowCredentials()
               .WithExposedHeaders("Content-Disposition"));

            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            ApplyMigrations(app);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ApplyMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }
}
