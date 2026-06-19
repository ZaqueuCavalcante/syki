using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Syki.Back.Database.Identity;

public class DataProtectionKeyDbConfig : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> entity)
    {
        entity.ToTable("data_protection_keys", DbSchemas.Syki);
    }
}
