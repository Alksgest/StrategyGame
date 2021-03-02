using Assets.Scripts.Behaviour.Unit;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Building
{
    public interface IWorkplace
    {
        string WorkKind { get; }

        GameObject ObjectToRotate { get; }

        void AttacheUnit(IWorkable unit);
        void DetachUnit(IWorkable unit);

        Vector3? GetFreePosition();
        Vector3? GetAttachedUnitPosition(IWorkable unit);
    }
}