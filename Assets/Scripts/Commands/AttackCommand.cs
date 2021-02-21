using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AttackCommand<T> : ICommand<T>, IRejectableCommand<T> where T : IAttacker
    {
        private readonly GameObject _target;

        public AttackCommand(GameObject target)
        {
            _target = target;
        }

        public void Execute(T obj)
        {
            obj.Attack(_target);
        }

        public void Reject(T obj)
        {
            obj.StopAttacking();
        }
    }
}
