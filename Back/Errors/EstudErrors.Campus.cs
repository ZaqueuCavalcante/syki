namespace Estud.Back.Errors;

public class CampusNotFound : EstudError
{
    public static readonly CampusNotFound I = new();
    public override string Code { get; set; } = nameof(CampusNotFound);
    public override string Message { get; set; } = "Campus não encontrado.";
}

public class InvalidCampusName : EstudError
{
    public static readonly InvalidCampusName I = new();
    public override string Code { get; set; } = nameof(InvalidCampusName);
    public override string Message { get; set; } = "Nome de campus inválido.";
}

public class InvalidCampusCity : EstudError
{
    public static readonly InvalidCampusCity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCity);
    public override string Message { get; set; } = "Cidade do campus inválida.";
}

public class InvalidBrazilState : EstudError
{
    public static readonly InvalidBrazilState I = new();
    public override string Code { get; set; } = nameof(InvalidBrazilState);
    public override string Message { get; set; } = "Estado inválido.";
}

public class InvalidCampusCapacity : EstudError
{
    public static readonly InvalidCampusCapacity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCapacity);
    public override string Message { get; set; } = "Capacidade inválida (deve ser maior que zero).";
}

public class InvalidCampusList : EstudError
{
    public static readonly InvalidCampusList I = new();
    public override string Code { get; set; } = nameof(InvalidCampusList);
    public override string Message { get; set; } = "Lista de campus inválida.";
}
