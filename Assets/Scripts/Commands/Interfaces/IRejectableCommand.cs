namespace Assets.Scripts.Commands.Interfaces
{
    public interface IRejectableCommand<T>
    {
        void Reject(T obj);
    }
}
