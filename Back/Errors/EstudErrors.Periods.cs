namespace Estud.Back.Errors;

public class AcademicPeriodNotFound : EstudError
{
    public static readonly AcademicPeriodNotFound I = new();
    public override string Code { get; set; } = nameof(AcademicPeriodNotFound);
    public override string Message { get; set; } = "Período acadêmico não encontrado.";
}

public class InvalidAcademicPeriod : EstudError
{
    public static readonly InvalidAcademicPeriod I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriod);
    public override string Message { get; set; } = "Período acadêmico inválido.";
}

public class InvalidAcademicPeriodStartDate : EstudError
{
    public static readonly InvalidAcademicPeriodStartDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodStartDate);
    public override string Message { get; set; } = "Data de início de período acadêmico inválida.";
}

public class InvalidAcademicPeriodEndDate : EstudError
{
    public static readonly InvalidAcademicPeriodEndDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodEndDate);
    public override string Message { get; set; } = "Data de fim de período acadêmico inválida.";
}

public class InvalidAcademicPeriodDates : EstudError
{
    public static readonly InvalidAcademicPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}

public class InvalidEnrollmentPeriodDates : EstudError
{
    public static readonly InvalidEnrollmentPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidEnrollmentPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}

public class AcademicPeriodAlreadyExists : EstudError
{
    public static readonly AcademicPeriodAlreadyExists I = new();
    public override string Code { get; set; } = nameof(AcademicPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período acadêmico com esse id.";
}

public class EnrollmentPeriodAlreadyExists : EstudError
{
    public static readonly EnrollmentPeriodAlreadyExists I = new();
    public override string Code { get; set; } = nameof(EnrollmentPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período de matrícula com esse nome.";
}

public class NoCurrentEnrollmentPeriod : EstudError
{
    public static readonly NoCurrentEnrollmentPeriod I = new();
    public override string Code { get; set; } = nameof(NoCurrentEnrollmentPeriod);
    public override string Message { get; set; } = "Não há período de matrícula vigente.";
}
