using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttachToWorkCommand<T> :IRejectableCommand<T> where T : IAttachable //, IWorkable
    {
        private readonly IWorkplace _workplace;

        public bool Interrupt { get; protected set; }

        public AttachToWorkCommand(IWorkplace workplace, bool interrupt = false)
        {
            _workplace = workplace;
            Interrupt = interrupt;
        }

        public bool Execute(T obj)
        {
            if (obj is IWorkable w)
            {
                return w.AttachToWork(_workplace);
            }

            return true;
        }

        public void Reject(T obj)
        {
            if (obj is IWorkable w)
            {
                w.DetachFromWork(_workplace);
            }
        }
    }
}
