using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class DataSourceProfilerConfigDbConfig : IEntityTypeConfiguration<DataSourceProfilerConfig>
{
    public void Configure(EntityTypeBuilder<DataSourceProfilerConfig> entity)
    {
        entity.ToTable("data_source_profiler_config", "public");

        entity.HasKey(e => e.DataSourceId)
            .HasName("data_source_profiler_config_pk");

        entity.Property(e => e.DataSourceId)
            .ValueGeneratedNever()
            .HasColumnName("data_source_id");

        entity.Property(e => e.MeasureProcessorTime)
            .HasDefaultValue(false)
            .HasColumnName("measure_processor_time");

        entity.Property(e => e.ProfilerEnable)
            .HasDefaultValue(false)
            .HasColumnName("profiler_enable");
    }
}
