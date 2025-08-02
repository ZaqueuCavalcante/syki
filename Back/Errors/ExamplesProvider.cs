using System.Reflection;

namespace Syki.Back.Errors;

public abstract class ExamplesProvider<T> : IMultipleExamplesProvider<T>
{
    public IEnumerable<SwaggerExample<T>> GetExamples()
    {
        var method = typeof(T).GetMethod("GetExamples", BindingFlags.Public | BindingFlags.Static);

        if (method == null)
            throw new InvalidOperationException($"{typeof(T).Name} must have a public static GetExamples() method.");

        if (method.Invoke(null, null) is not IEnumerable<(string, T)> examples)
            throw new InvalidOperationException($"GetExamples() on {typeof(T).Name} must return IEnumerable<(string, {typeof(T).Name})>");

        return examples.Select(e => SwaggerExample.Create(e.Item1, e.Item2));
    }
}
