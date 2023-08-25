using Syki.Domain;
using Microsoft.EntityFrameworkCore;

namespace Syki.Database;

public class SykiDbContext : DbContext
{
    public DbSet<Faculdade> Faculdades { get; set; }
    public DbSet<Aluno> Alunos { get; set; }

    public SykiDbContext(DbContextOptions<SykiDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("syki");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
