namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class UserRegisterConfig : IEntityTypeConfiguration<UserRegister>
{
    public void Configure(EntityTypeBuilder<UserRegister> register)
    {
        register.ToTable("user_registers");

        register.HasKey(r => r.Id);

        register.HasIndex(r => r.Email).IsUnique();
    }
}