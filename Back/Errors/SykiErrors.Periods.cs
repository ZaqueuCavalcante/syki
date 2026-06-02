namespace Syki.Back.Errors;

public class AcademicPeriodNotFound : SykiError
{
    public static readonly AcademicPeriodNotFound I = new();
    public override string Code { get; set; } = nameof(AcademicPeriodNotFound);
    public override string Message { get; set; } = "Período acadêmico não encontrado.";
}

public class InvalidAcademicPeriod : SykiError
{
    public static readonly InvalidAcademicPeriod I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriod);
    public override string Message { get; set; } = "Período acadêmico inválido.";
}

public class InvalidAcademicPeriodStartDate : SykiError
{
    public static readonly InvalidAcademicPeriodStartDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodStartDate);
    public override string Message { get; set; } = "Data de início de período acadêmico inválida.";
}

public class InvalidAcademicPeriodEndDate : SykiError
{
    public static readonly InvalidAcademicPeriodEndDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodEndDate);
    public override string Message { get; set; } = "Data de fim de período acadêmico inválida.";
}

public class InvalidAcademicPeriodDates : SykiError
{
    public static readonly InvalidAcademicPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}

public class InvalidEnrollmentPeriodDates : SykiError
{
    public static readonly InvalidEnrollmentPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidEnrollmentPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}

public class AcademicPeriodAlreadyExists : SykiError
{
    public static readonly AcademicPeriodAlreadyExists I = new();
    public override string Code { get; set; } = nameof(AcademicPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período acadêmico com esse id.";
}
