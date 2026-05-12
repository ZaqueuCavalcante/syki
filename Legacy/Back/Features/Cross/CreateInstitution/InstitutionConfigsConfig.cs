namespace Syki.Back.Features.Cross.CreateInstitution;

public class InstitutionConfigsConfig : IEntityTypeConfiguration<InstitutionConfigs>
{
    public void Configure(EntityTypeBuilder<InstitutionConfigs> configs)
    {
        configs.ToTable("institution_configs");

        configs.HasKey(t => t.InstitutionId);
        configs.Property(t => t.InstitutionId).ValueGeneratedNever();

        configs.Property(x => x.FrequencyLimit).HasPrecision(4, 2);
    }
}
