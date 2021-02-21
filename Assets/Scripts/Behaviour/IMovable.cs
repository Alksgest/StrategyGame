using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public interface IMovable
    {
        void Move(Vector3 position);
        void StopMoving();
    }
}