using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class MoveCommand<T> : ICommand<T>, IRejectableCommand<T> where T : IMovable
    {
        private readonly Vector3 _vector;

        public MoveCommand(Vector3 vector)
        {
            _vector = vector;
        }

        public void Execute(T obj)
        {
            obj.Move(_vector);
        }

        public void Reject(T obj)
        {
            obj.StopMoving();
        }
    }
}