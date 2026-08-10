using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Services;

var builder = WebApplication.CreateBuilder(args);

// FIX STATUS 139: Desactiva el monitoreo continuo de archivos (inotify) en Linux/Docker
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddHttpClient<TelegramService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuración de MySQL para Aiven con reintentos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
        );
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ejecución segura de migraciones
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Migraciones ejecutadas exitosamente en Aiven MySQL.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al ejecutar migraciones: {ex.Message}");
    }
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();