namespace Assets.Scripts.Commands.Interfaces
{
    public interface ICommand<in T>
    {
        void Execute(T obj);
    }
}