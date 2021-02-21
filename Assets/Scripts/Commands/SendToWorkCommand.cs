using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class SendToWorkCommand<T> : ICommand<T>, IRejectableCommand<T> where T : IAttachable, IWorkable
    {
        private readonly IWorkplace _workplace;

        public SendToWorkCommand(IWorkplace workplace)
        {
            _workplace = workplace;
        }

        public void Execute(T obj)
        {
            obj.AttachToWork(_workplace);
        }

        public void Reject(T obj)
        {
            obj.DetachFromWork(_workplace);
        }
    }
}
