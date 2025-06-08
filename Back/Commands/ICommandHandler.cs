namespace Syki.Back.Commands;

public interface ICommandHandler<T> where T : ICommand
{
    // TODO: Criar uma forma de passar esse commandId automaticamente
    Task Handle(CommandId commandId, T command);
}
