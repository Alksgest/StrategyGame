namespace Assets.Scripts.Commands.Interfaces
{
    public interface IRejectableCommand<in T>
    {
        void Reject(T obj);
    }
}
