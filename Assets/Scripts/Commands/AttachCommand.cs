using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttachCommand<T> : IRejectableCommand<T> where T : IAttachable
    {
        private readonly GameObject _objectAttachedTo;

        public bool Interrupt { get; protected set; }

        public AttachCommand(GameObject objectAttachedTo, bool interrupt = false)
        {
            _objectAttachedTo = objectAttachedTo;
            Interrupt = interrupt;
        }

        public bool Execute(T obj)
        {
            return obj.Attach(_objectAttachedTo);
        }

        public void Reject(T obj)
        {
            obj.Detach();
        }
    }
}
