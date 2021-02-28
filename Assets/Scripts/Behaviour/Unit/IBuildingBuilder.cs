using Assets.Scripts.Behaviour.Building;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IBuildingBuilder
    {
        bool Build(IBuildable building);
        void DetachFromBuilding();
    }
}