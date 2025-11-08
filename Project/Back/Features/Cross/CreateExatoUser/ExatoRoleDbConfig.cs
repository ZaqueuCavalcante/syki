using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoRoleDbConfig : IEntityTypeConfiguration<ExatoRole>
{
    public void Configure(EntityTypeBuilder<ExatoRole> entity)
    {
        entity.ToTable("roles", "exato");

        entity.Property(e => e.Description)
            .IsRequired();

        entity.Property(e => e.OrganizationId)
            .IsRequired();

        entity.Property(e => e.Features)
            .HasColumnType("integer[]")
            .IsRequired();

        entity.HasIndex(e => new { e.OrganizationId, e.NormalizedName })
            .IsUnique();






        // entity.HasData(new ExatoRole(Guid.Parse("5912ebe1-9e6a-4ce1-90bf-8490534fb4e1"), "OfficeAdm", "OfficeAdm", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("61a1cd29-f513-4a25-9be0-f47d6aef90e7"), "OfficeSupport", "OfficeSupport", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("95b27c30-f027-4971-b715-22a2e1f138fe"), "OfficeFinance", "OfficeFinance", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("78691a7a-f554-42d7-a5cf-8d474b6de0dd"), "OfficeCustomerSuccess", "OfficeCustomerSuccess", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("0199d4cf-133f-7bcb-b37d-06e2c30ebbc9"), "OfficeCustomerSuccessManager", "OfficeCustomerSuccessManager", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("a0acdbb6-eab8-40dd-af9a-e76134dd9445"), "OrgAdm", "OrgAdm", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("588188bd-b870-454a-b9e6-5bb005e9a5bf"), "OrgRecruiter", "OrgRecruiter", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("d33c08ac-737a-4076-8678-e2cbe157c450"), "OrgManager", "OrgManager", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("3c076119-c6ca-44c9-86e1-81785664b8b5"), "OrgFinance", "OrgFinance", 1, []));
        // entity.HasData(new ExatoRole(Guid.Parse("7a8ee2ef-d8e7-499e-be2f-967ac20092bf"), "OrgCandidate", "OrgCandidate", 1, []));
    }
}
