using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class MoveCommand<T> : IRejectableCommand<T> where T : IMovable
    {
        public bool Interrupt { get; protected set; }

        private readonly Vector3 _vector;

        public MoveCommand(Vector3 vector, bool interrupt = true)
        {
            _vector = vector;
            Interrupt = interrupt;
        }

        public bool Execute(T source)
        {
            return source.Move(_vector);
        }

        public void Reject(T source)
        {
            source.StopMoving();
        }
    }
}