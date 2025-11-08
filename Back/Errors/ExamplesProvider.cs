namespace Syki.Back.Errors;

public abstract class ExamplesProvider<T> : IMultipleExamplesProvider<T> where T : IApiDto<T>
{
    public IEnumerable<SwaggerExample<T>> GetExamples()
    {
        foreach (var (name, value) in T.GetExamples())
            yield return SwaggerExample.Create(name, value);
    }
}
