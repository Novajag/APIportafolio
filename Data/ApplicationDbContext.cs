using Microsoft.EntityFrameworkCore;
using Portafolio.Models;

namespace Portafolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Esta línea creará la tabla "MensajesContacto" en MySQL
        public DbSet<MessageContact> Messages { get; set; }
    }
}