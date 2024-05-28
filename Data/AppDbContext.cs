using Microsoft.EntityFrameworkCore;
using MinimalAPI.Estudandes;

namespace MinimalAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<Estudante> Estudantes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.Sqlite");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        base.OnConfiguring(optionsBuilder);
    }
}