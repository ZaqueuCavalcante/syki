namespace Exato.Shared.Interfaces;

public interface IApiDto<TSelf> where TSelf : IApiDto<TSelf>
{
    static abstract IEnumerable<(string Name, TSelf Value)> GetExamples();
}
