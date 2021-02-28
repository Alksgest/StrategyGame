using UnityEngine;

namespace Assets.Scripts.Behaviour.Building
{
    public interface IBuildable
    {
        float BuildingProgress { get; }
        Vector3 Destination { get; }
    }
}