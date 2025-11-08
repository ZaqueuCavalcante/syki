namespace Exato.Back.Intelligence.Domain.Public;

public class OrganizationUser
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeavedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public bool ItsHisOwn { get; set; }

    public string? IndicationCode { get; set; }

    public OrganizationUser() { }

    public OrganizationUser(int clienteId, int userId, bool itsHisOwn)
    {
        ClienteId = clienteId;
        UserId = userId;
        JoinedAt = DateTime.Now;
        CreatedAt = DateTime.Now;
        CreatedBy = 7;
        ItsHisOwn = itsHisOwn;
    }

    public void SoftDelete()
    {
        LeavedAt = DateTime.Now;
    }
}
