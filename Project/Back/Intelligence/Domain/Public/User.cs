using System.Net;

namespace Exato.Back.Intelligence.Domain.Public;

public class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string EncryptedPassword { get; set; }

    public string? ResetPasswordToken { get; set; }

    public DateTime? ResetPasswordSentAt { get; set; }

    public DateTime? RememberCreatedAt { get; set; }

    public int SignInCount { get; set; }

    public DateTime? CurrentSignInAt { get; set; }

    public DateTime? LastSignInAt { get; set; }

    public IPAddress? CurrentSignInIp { get; set; }

    public IPAddress? LastSignInIp { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Provider { get; set; }

    public string? Uid { get; set; }

    public string? InvitationToken { get; set; }

    public DateTime? InvitationCreatedAt { get; set; }

    public DateTime? InvitationSentAt { get; set; }

    public DateTime? InvitationAcceptedAt { get; set; }

    public int? InvitationLimit { get; set; }

    public int? InvitedById { get; set; }

    public string? InvitedByType { get; set; }

    public int? InvitationsCount { get; set; }

    public string? FullName { get; set; }

    public int? ClienteId { get; set; }

    public bool Active { get; set; }

    public bool Internal { get; set; }

    public bool? Visible { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public Guid? ExternalId { get; set; }

    public long? Cpf { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public short RealmId { get; set; }

    public short? OrigemId { get; set; }

    public DateOnly? MigratedAt { get; set; }

    public Guid? MigratedToUserExternalId { get; set; }

    public string? Origin { get; set; }

    public User() { }

    public User(int clienteId, string name, string email, string? cpf)
    {
        ClienteId = clienteId;
        FullName = name;
        Email = email;
        Cpf = cpf.OnlyNumbers().HasValue() ? long.Parse(cpf.OnlyNumbers()) : null;
        Active = true;
        Internal = false;
        Visible = true;
        CreatedBy = 1;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        RealmId = 1;
        ExternalId = Guid.NewGuid();
    }

    public void EditarCadastro(string name, string email, string? cpf)
    {
        FullName = name;
        Email = email;
        Cpf = cpf.OnlyNumbers().HasValue() ? long.Parse(cpf.OnlyNumbers()) : null;
        UpdatedAt = DateTime.Now;
    }

    public string? GetCpf()
    {
        if (Cpf == null) return null;

        return Cpf.ToString().PadLeft(11, '0');
    }

    public void SoftDelete()
    {
        Active = false;
        DeletedAt = DateTime.Now;
        DeletedBy = 7;
    }
}
