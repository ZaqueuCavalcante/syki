namespace Syki.Back.CreatePendingUserRegister;

public class UserRegisterConfig : IEntityTypeConfiguration<UserRegister>
{
    public void Configure(EntityTypeBuilder<UserRegister> register)
    {
        register.ToTable("user_registers");

        register.HasKey(r => r.Id);

        register.HasIndex(r => r.Email).IsUnique();
        register.Property(r => r.TrialStart);
        register.Property(r => r.TrialEnd);
    }
}
