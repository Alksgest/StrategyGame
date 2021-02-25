using UnityEngine;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IMovable
    {
        // bool indicate that unit is on his destination
        bool Move(Vector3 position);
        void StopMoving();
    }
}