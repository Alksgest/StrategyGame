using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttackCommand<T> : IRejectableCommand<T> where T : IAttacker
    {
        private readonly GameObject _target;

        public bool Interrupt { get; protected set; }

        public AttackCommand(GameObject target, bool interrupt = true)
        {
            _target = target;
            Interrupt = interrupt;
        }

        public bool Execute(T obj)
        {
           return obj.Attack(_target);
        }

        public void Reject(T obj)
        {
            obj.StopAttacking();
        }
    }
}
