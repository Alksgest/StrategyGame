using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttachCommand<T> : ICommand<T>, IRejectableCommand<T> where T : IAttachable
    {

        private readonly GameObject _objectAttachedTo;

        public AttachCommand(GameObject objectAttachedTo)
        {
            _objectAttachedTo = objectAttachedTo;
        }

        public void Execute(T obj)
        {
            obj.Attach(_objectAttachedTo);
        }

        public void Reject(T obj)
        {
            obj.Detach();
        }
    }
}
