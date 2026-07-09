namespace Estud.Back.Errors;

public abstract class ErrorExamplesProvider<T1> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new[] { new T1() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
    where T7 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
    where T7 : EstudError, new()
    where T8 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
    where T7 : EstudError, new()
    where T8 : EstudError, new()
    where T9 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8(), new T9() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
    where T7 : EstudError, new()
    where T8 : EstudError, new()
    where T9 : EstudError, new()
    where T10 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8(), new T9(), new T10() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IMultipleExamplesProvider<ErrorOut>
    where T1 : EstudError, new()
    where T2 : EstudError, new()
    where T3 : EstudError, new()
    where T4 : EstudError, new()
    where T5 : EstudError, new()
    where T6 : EstudError, new()
    where T7 : EstudError, new()
    where T8 : EstudError, new()
    where T9 : EstudError, new()
    where T10 : EstudError, new()
    where T11 : EstudError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new EstudError[] { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8(), new T9(), new T10(), new T11() }.Select(e => e.ToSwaggerExampleErrorOut());
}
