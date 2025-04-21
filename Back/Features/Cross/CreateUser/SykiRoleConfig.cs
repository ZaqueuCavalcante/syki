namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRoleConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> role)
    {
        role.HasData(new SykiRole(Guid.Parse("5912ebe1-9e6a-4ce1-90bf-8490534fb4e1"), UserRole.Adm));
        role.HasData(new SykiRole(Guid.Parse("78691a7a-f554-42d7-a5cf-8d474b6de0dd"), UserRole.Academic));
        role.HasData(new SykiRole(Guid.Parse("ca6f394f-6fd9-48cc-90a8-b379636a24e7"), UserRole.Teacher));
        role.HasData(new SykiRole(Guid.Parse("f9ad5139-06c3-4ce2-9748-ecc498b087c7"), UserRole.Student));
    }
}
