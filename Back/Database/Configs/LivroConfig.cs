using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class LivroConfig : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> livro)
    {
        livro.ToTable("livros");

        livro.HasKey(l => l.Id);
        livro.Property(l => l.Id).ValueGeneratedNever();
    }
}
