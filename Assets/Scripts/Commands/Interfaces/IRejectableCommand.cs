namespace Assets.Scripts.Commands.Interfaces
{
    public interface IRejectableCommand<in T> : ICommand<T>
    {
        void Reject(T source);
    }
}
