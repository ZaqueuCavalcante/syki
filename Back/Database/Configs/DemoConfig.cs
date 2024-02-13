using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class DemoConfig : IEntityTypeConfiguration<Demo>
{
    public void Configure(EntityTypeBuilder<Demo> demo)
    {
        demo.ToTable("demos");

        demo.HasKey(d => d.Id);
        demo.Property(c => c.Id).ValueGeneratedNever();

        demo.HasIndex(d => d.Email).IsUnique();
    }
}
