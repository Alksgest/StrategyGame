using Assets.Scripts.Unit.Interfaces;

namespace Assets.Scripts.Building.Interfaces
{
    public interface IWorkplace
    {
        void AttacheUnit(IWorkable unit, Workpalce edge);
        void DetachUnit(IWorkable unit);
    }
}