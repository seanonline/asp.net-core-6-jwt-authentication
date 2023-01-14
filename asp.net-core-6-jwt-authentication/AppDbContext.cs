using asp.net_core_6_jwt_authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_6_jwt_authentication
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }


        public DbSet<LoginResponse> Users { get; set; }


        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Metadata.FindProperty("ModifiedAt") != null)
                {
                    entityEntry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
                }
                if (entityEntry.Metadata.FindProperty("CreatedAt") != null)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        //entityEntry.Property("UUID").CurrentValue = Guid.NewGuid();
                        entityEntry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
