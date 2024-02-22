using Microsoft.EntityFrameworkCore;
using WebAggregator.Repository.Entities;

namespace WebAggregator.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WebPage> WebPages { get; set; }
}
