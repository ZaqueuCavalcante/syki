namespace Syki.Back.Errors;

public abstract class ErrorExamplesProvider<T1> : IMultipleExamplesProvider<ErrorOut>
    where T1 : SykiError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new[] { new T1() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2> : IMultipleExamplesProvider<ErrorOut>
    where T1 : SykiError, new()
    where T2 : SykiError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new SykiError[] { new T1(), new T2() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3> : IMultipleExamplesProvider<ErrorOut>
    where T1 : SykiError, new()
    where T2 : SykiError, new()
    where T3 : SykiError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new SykiError[] { new T1(), new T2(), new T3() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4> : IMultipleExamplesProvider<ErrorOut>
    where T1 : SykiError, new()
    where T2 : SykiError, new()
    where T3 : SykiError, new()
    where T4 : SykiError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new SykiError[] { new T1(), new T2(), new T3(), new T4() }.Select(e => e.ToSwaggerExampleErrorOut());
}

public abstract class ErrorExamplesProvider<T1, T2, T3, T4, T5> : IMultipleExamplesProvider<ErrorOut>
    where T1 : SykiError, new()
    where T2 : SykiError, new()
    where T3 : SykiError, new()
    where T4 : SykiError, new()
    where T5 : SykiError, new()
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples() =>
        new SykiError[] { new T1(), new T2(), new T3(), new T4(), new T5() }.Select(e => e.ToSwaggerExampleErrorOut());
}
