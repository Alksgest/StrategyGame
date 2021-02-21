using UnityEngine;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IMovable
    {
        void Move(Vector3 position);
        void StopMoving();
    }
}