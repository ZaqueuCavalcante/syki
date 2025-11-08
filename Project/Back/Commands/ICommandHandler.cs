namespace Exato.Back.Commands;

public interface ICommandHandler<T> where T : ICommand
{
    // TODO: Criar uma forma de passar esse commandId automaticamente (WorkerContext? ProcessorContext?)
    Task Handle(Guid commandId, T command);
}
