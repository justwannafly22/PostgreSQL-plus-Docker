using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAggregator.Infrastructure;
using WebAggregator.Infrastructure.Extensions;
using WebAggregator.Infrastructure.Middleware;
using WebAggregator.Repository;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Services.AddControllers();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("sqlConnection")));

    builder.Services.ConfigureLogic();
    builder.Services.ConfigureFactories();
    builder.Services.ConfigureRepositories();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.UseMiddleware<ExceptionHandler>();

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