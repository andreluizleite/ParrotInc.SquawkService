using Microsoft.EntityFrameworkCore;
using ParrotInc.SquawkService.Infrastructure.Entity;
public class MyDbContext : DbContext
{
    public DbSet<SquawkEventEntity> SquawkEvents { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }
}