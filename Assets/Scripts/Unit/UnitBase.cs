using System;
using System.Collections;
using System.Numerics;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Animation;
using Assets.Scripts.Models.Unit;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        [SerializeField] protected Animator Animator;

        protected GameObject AttackTarget;
        protected UnitStats PreviousStats;
        protected GameObject ObjectAttachedTo;
        protected IRejectableCommand<UnitBase> LastRejectableCommand;

        protected bool IsAttackCoroutineDone = true;

        public bool Selected { get; protected set; } = false;
        public bool IsAlive => CurrentStats.Health > 0;

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
            if (target == null)
            {
                RejectLastCommand();
                return;
            }

            if (AttackTarget == null)
            {
                AttackTarget = target;
            }

            var ias = target.GetComponent<IAttackSusceptible>();
            if (ias == null)
            {
                RejectLastCommand();
                return;
            }

            var attackPosition = AttackTarget.transform.position -
                                 new Vector3(CurrentStats.AttackRange, 0, CurrentStats.AttackRange);

            var routine = MakeDamage(ias);

            if (transform.position != attackPosition)
            {
                StopCoroutine(routine);
                Move(AttackTarget.transform.position -
                     new Vector3(CurrentStats.AttackRange, 0, CurrentStats.AttackRange));
            }
            else if (IsAttackCoroutineDone)
            {
                StartCoroutine(routine);
            }
        }

        private IEnumerator MakeDamage(IAttackSusceptible ias)
        {
            IsAttackCoroutineDone = false;
            yield return new WaitForSeconds(CurrentStats.AttackSpeed / 2);
            ias.TakeDamage(CurrentStats.Attack);
            yield return new WaitForSeconds(CurrentStats.AttackSpeed / 2);
            IsAttackCoroutineDone = true;
        }

        public void StopAttacking()
        {
            AttackTarget = null;
        }

        public virtual void Move(Vector3 point)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(point);
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.Walking, true);
            }

            var x = transform.position.x;
            var z = transform.position.z;
            if (Math.Abs(x - point.x) < 0.1 && Math.Abs(z - point.z) < 0.1)
            {
                RejectLastCommand();
            }
        }

        public virtual void StopMoving()
        {
            NavMeshAgent.isStopped = true;
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.Walking, false);
            }
        }

        public virtual void Select()
        {
            Selected = true;
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = SelectedMaterial;
            }

            UnitUi.SetActive(true);
        }

        public virtual void Deselect()
        {
            Selected = false;
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = DefaultMaterial;
            }

            UnitUi.SetActive(false);
        }

        public virtual void TakeDamage(float value)
        {
            var damage = (value - CurrentStats.Armor) / CurrentStats.Defence;
            PreviousStats = UnitStats.MakeCopy(CurrentStats);
            CurrentStats.Health -= damage;
        }

        public virtual void Instantiate(UnitStats stats)
        {
            CurrentStats = UnitStats.MakeCopy(stats);
            PreviousStats = UnitStats.MakeCopy(CurrentStats);
        }

        public virtual void Delete()
        {
            Destroy(gameObject);
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