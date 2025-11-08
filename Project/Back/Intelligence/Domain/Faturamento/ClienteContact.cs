namespace Exato.Back.Intelligence.Domain.Faturamento;

public class ClienteContact
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime InsertedAt { get; set; }

    public bool Active { get; set; }

    public string? AdditionalData { get; set; }
}
