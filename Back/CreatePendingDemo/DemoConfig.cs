using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.CreatePendingDemo;

public class DemoConfig : IEntityTypeConfiguration<Demo>
{
    public void Configure(EntityTypeBuilder<Demo> demo)
    {
        demo.ToTable("demos");

        demo.HasKey(d => d.Id);
        demo.Property(d => d.Id).ValueGeneratedNever();

        demo.HasIndex(d => d.Email).IsUnique();
        demo.Property(d => d.Start);
        demo.Property(d => d.End);
    }
}
