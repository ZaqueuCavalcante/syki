namespace Estud.Back.Domain.Classes;

[DomainEvent("ClassActivity", "Atividade criada")]
public record ClassActivityCreated(string Uid) : IDomainEvent;
