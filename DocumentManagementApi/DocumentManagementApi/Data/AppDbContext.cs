using DocumentManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagementApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppFile> File { get; set; }
    }
}
