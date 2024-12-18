namespace Syki.Back.Tasks;

public interface ISykiTaskHandler<T> where T : ISykiTask
{
    Task Handle(T task);
}
