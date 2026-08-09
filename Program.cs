using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<TelegramService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Fijamos la versión explícita de MariaDB/MySQL en lugar de usar AutoDetect
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31))));

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

// Envolvemos las migraciones en un bloque try-catch para evitar que el proceso colapse si la BD tarda en responder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
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