namespace Syki.Shared;

public interface IApiDto<TSelf> where TSelf : IApiDto<TSelf>
{
    static abstract IEnumerable<(string Name, TSelf Value)> GetExamples();
}
