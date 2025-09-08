using Microsoft.EntityFrameworkCore;

namespace taller_backend.ContextDB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // No agregamos DbSets todavía hasta que tengamos los modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Las configuraciones de modelos se agregarán después
        }
    }
}
