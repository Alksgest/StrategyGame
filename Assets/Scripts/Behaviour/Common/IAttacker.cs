using UnityEngine;

namespace Assets.Scripts.Behaviour.Common
{
    public interface IAttacker
    {
        bool Attack(GameObject target);
        void StopAttacking();
    }
}
