using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Building;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Building
{
    public interface IWorkplace
    {
        void AttacheUnit(IWorkable unit);
        void DetachUnit(IWorkable unit);

        Vector3? GetFreePosition();
    }
}