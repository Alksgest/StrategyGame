namespace Assets.Scripts.Commands.Interfaces
{
    public interface ICommandExecutor<out T>
    {
        void Execute(ICommand<T> command);
    }
}