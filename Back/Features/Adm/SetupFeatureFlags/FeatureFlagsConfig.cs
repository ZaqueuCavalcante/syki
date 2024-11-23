namespace Syki.Back.Features.Cross.SetupFeatureFlags;

public class FeatureFlagsConfig : IEntityTypeConfiguration<FeatureFlags>
{
    public void Configure(EntityTypeBuilder<FeatureFlags> features)
    {
        features.ToTable("feature_flags");

        features.HasKey(f => f.Id);
        features.Property(f => f.Id).ValueGeneratedNever();
    }
}
