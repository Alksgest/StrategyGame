
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class RotateCommand<T> : IRejectableCommand<T> where T : IRotatable
    {
        public bool Interrupt { get; }

        private readonly Vector3 _target;

        public RotateCommand(Vector3 target, bool interrupt = false)
        {
            Interrupt = interrupt;
            _target = target;
        }

        public bool Execute(T obj)
        {
            return obj.Rotate(_target);
        }

        public void Reject(T source)
        {
        }
    }
}
