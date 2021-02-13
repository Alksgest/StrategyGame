using StrategyGame.Assets.Scripts.Unit.Interfaces;

namespace StrategyGame.Assets.Scripts.Building.Interfaces
{
    public interface IWorkplace
    {
        void AttacheUnit(IWorkable unit, Workpalce edge);
        void DeatachUnit(IWorkable unit);
    }
}