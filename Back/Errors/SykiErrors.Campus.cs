namespace Syki.Back.Errors;

public class CampusNotFound : SykiError
{
    public static readonly CampusNotFound I = new();
    public override string Code { get; set; } = nameof(CampusNotFound);
    public override string Message { get; set; } = "Campus não encontrado.";
}

public class InvalidCampusName : SykiError
{
    public static readonly InvalidCampusName I = new();
    public override string Code { get; set; } = nameof(InvalidCampusName);
    public override string Message { get; set; } = "Nome de campus inválido.";
}

public class InvalidCampusCity : SykiError
{
    public static readonly InvalidCampusCity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCity);
    public override string Message { get; set; } = "Cidade do campus inválida.";
}

public class InvalidBrazilState : SykiError
{
    public static readonly InvalidBrazilState I = new();
    public override string Code { get; set; } = nameof(InvalidBrazilState);
    public override string Message { get; set; } = "Estado inválido.";
}

public class InvalidCampusCapacity : SykiError
{
    public static readonly InvalidCampusCapacity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCapacity);
    public override string Message { get; set; } = "Capacidade inválida (deve ser maior que zero).";
}
