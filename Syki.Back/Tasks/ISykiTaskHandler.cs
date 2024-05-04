namespace Syki.Back.Tasks;

public interface ISykiTaskHandler<T>
{
    Task Handle(T task);
}
