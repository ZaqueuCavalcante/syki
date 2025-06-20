using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Syki.Back.Metrics;

public class RequestMetricsConfig : IEntityTypeConfiguration<RequestMetrics>
{
    public void Configure(EntityTypeBuilder<RequestMetrics> requestMetrics)
    {
        requestMetrics.ToTable("request_metrics");

        requestMetrics.HasKey(w => w.Id);
        requestMetrics.Property(w => w.Id).ValueGeneratedNever();

        requestMetrics.OwnsOne(x => x.Resume).ToJson();

        requestMetrics.OwnsMany(x => x.Requests, builder =>
        {
            builder.ToJson();
            builder.Property(z => z.Values)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<ConcurrentDictionary<string, int>>(v) ?? new ConcurrentDictionary<string, int>());
        });
    }
}
