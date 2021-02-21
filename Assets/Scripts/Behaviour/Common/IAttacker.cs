using UnityEngine;

namespace Assets.Scripts.Behaviour.Common
{
    public interface IAttacker
    {
        void Attack(GameObject target);
        void StopAttacking();
    }
}
