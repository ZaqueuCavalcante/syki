namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRoleConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> role)
    {
        role.HasData(new SykiRole(UserRole.Adm));
        role.HasData(new SykiRole(UserRole.Academic));
        role.HasData(new SykiRole(UserRole.Teacher));
        role.HasData(new SykiRole(UserRole.Student));
        role.HasData(new SykiRole(UserRole.Seller));
    }
}
