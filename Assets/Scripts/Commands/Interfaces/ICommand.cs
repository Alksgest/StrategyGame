namespace Assets.Scripts.Commands.Interfaces
{
    public interface ICommand<in T>
    {
        bool Interrupt { get; }
        bool Execute(T obj);
    }
}