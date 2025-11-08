namespace Exato.Back.Errors;

public abstract class ErrorExamplesProvider<T1> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new[] { new T1() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
    where T6 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
    where T6 : ExatoError, new()
    where T7 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
    where T6 : ExatoError, new()
    where T7 : ExatoError, new()
    where T8 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
    where T6 : ExatoError, new()
    where T7 : ExatoError, new()
    where T8 : ExatoError, new()
    where T9 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8(), new T9() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IMultipleExamplesProvider<ErrorOut>
    where T1 : ExatoError, new()
    where T2 : ExatoError, new()
    where T3 : ExatoError, new()
    where T4 : ExatoError, new()
    where T5 : ExatoError, new()
    where T6 : ExatoError, new()
    where T7 : ExatoError, new()
    where T8 : ExatoError, new()
    where T9 : ExatoError, new()
    where T10 : ExatoError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new ExatoError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8(), new T9(), new T10() }.Select(e => e.ToSwaggerExampleErrorOut());
}
