using System;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Unit
{
    public abstract class UnitBase : MonoBehaviour, IEquatable<UnitBase>, ICommandExecutor<UnitBase>, IUnit, IAttacker
    {
        [SerializeField] protected string UnitId;
        [SerializeField] protected Material DefaultMaterial;
        [SerializeField] protected Material SelectedMaterial;
        [SerializeField] protected GameObject UnitUi;
        [SerializeField] protected NavMeshAgent NavMeshAgent;
        [SerializeField] protected UnitStats CurrentStats;

        protected Animator Animator;
        protected GameObject AttackTarget;
        protected UnitStats PreviousStats;
        protected GameObject ObjectAttachedTo;
        protected IRejectableCommand<UnitBase> LastRejectableCommand;

        public bool Selected { get; protected set; } = false;

        public abstract void HideUi();

        public virtual void Execute(ICommand<UnitBase> command)
        {
            if (command is IRejectableCommand<UnitBase> r)
            {
                RejectLastCommand();
                LastRejectableCommand = r;
            }

            command.Execute(this);
        }

        protected virtual void ExecuteLastRejectableCommand()
        {
            (LastRejectableCommand as ICommand<UnitBase>)?.Execute(this);
        }

        protected virtual void RejectLastCommand()
        {
            LastRejectableCommand?.Reject(this);
            LastRejectableCommand = null;
        }

        public virtual void Attach(GameObject obj)
        {
            ObjectAttachedTo = obj;
        }

        public virtual void Detach()
        {
            ObjectAttachedTo = null;
        }

        public void Attack(GameObject target)
        {
            AttackTarget = target;
            Move(AttackTarget.transform.position - new Vector3(CurrentStats.AttackRange, 0, CurrentStats.AttackRange));
        }

        public void StopAttacking()
        {
            AttackTarget = null;
        }

        public virtual void Move(Vector3 point)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(point);
        }

        public virtual void StopMoving()
        {
            NavMeshAgent.isStopped = true;
        }

        public virtual void Select()
        {
            Selected = true;
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = SelectedMaterial;
            UnitUi.SetActive(true);
        }

        public virtual void Deselect()
        {
            Selected = false;
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = DefaultMaterial;
            UnitUi.SetActive(false);
        }

        //protected virtual void SetAttackTarget(GameObject target)
        //{
        //    AttackTarget = target;
        //}

        //protected virtual void Attack()
        //{
        //    Move(AttackTarget.transform.position - new Vector3(CurrentStats.AttackRange, 0, CurrentStats.AttackRange));
        //}

        public virtual void Instantiate(UnitStats stats)
        {
            CurrentStats = stats;
            PreviousStats = UnitStats.MakeCopy(stats);
        }

        public bool Equals(UnitBase other)
        {
            return UnitId == other?.UnitId;
        }

        public override int GetHashCode()
        {
            return UnitId.GetHashCode();
        }
    }
}