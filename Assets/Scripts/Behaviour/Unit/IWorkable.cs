using Assets.Scripts.Behaviour.Building;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IWorkable
    {
        bool AttachToWork(IWorkplace workplace);
        void DetachFromWork(IWorkplace workplace);
    }
}