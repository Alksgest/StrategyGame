using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Animation;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Unit
{
    public abstract class UnitBase : MonoBehaviour, IUnit, IAttacker, ICommandExecutor<UnitBase>
    {
        [SerializeField] public string UnitId;

        [SerializeField] protected GameObject UnitUi;
        [SerializeField] protected NavMeshAgent NavMeshAgent;
        [SerializeField] protected UnitStats CurrentStats;
        [SerializeField] protected Animator Animator;

        protected GameObject AttackTarget;
        protected UnitStats PreviousStats;
        protected GameObject ObjectAttachedTo;

        protected Queue<IRejectableCommand<UnitBase>> RejectableCommandsQueue =
            new Queue<IRejectableCommand<UnitBase>>();

        protected bool IsAttackCoroutineDone = true;

        public bool Selected { get; protected set; }
        public bool IsAlive => CurrentStats.Health > 0;

        public abstract void HideUi();

        public virtual void Execute(ICommand<UnitBase> command)
        {
            if (command is IRejectableCommand<UnitBase> r)
            {
                Debug.Log($"{command.GetType().Name} was enqueue to base queue");

                if (r.Interrupt && RejectableCommandsQueue.Any())
                {
                    RejectLastCommand();
                }

                RejectableCommandsQueue.Enqueue(r);

            }

            command.Execute(this);
        }

        protected virtual void ExecuteLastRejectableCommand()
        {
            if (RejectableCommandsQueue.Any())
            {
                var command = RejectableCommandsQueue.Peek() as ICommand<UnitBase>;
                var result = command.Execute(this);
                if (result)
                {
                    RejectLastCommand();
                }
            }
        }

        protected virtual void RejectLastCommand()
        {
            var command = RejectableCommandsQueue.Dequeue();
            Debug.Log($"{command.GetType().Name} was dequeue from base queue");
            command?.Reject(this);
        }

        public virtual bool Attach(GameObject obj)
        {
            ObjectAttachedTo = obj;
            return true;
        }

        public virtual void Detach()
        {
            ObjectAttachedTo = null;
        }

        public bool Attack(GameObject target)
        {
            if (target == null)
            {
                return true;
            }

            if (AttackTarget == null)
            {
                AttackTarget = target;
            }

            var ias = target.GetComponent<IAttackSusceptible>();
            if (ias == null)
            {
                return true;
            }
            
            var routine = MakeDamage(ias);

            var dist = Vector3.Distance(transform.position, AttackTarget.transform.position);

            // TODO: add attack range using
            if (dist > CurrentStats.AttackRange)
            {
                StopCoroutine(routine);
                Move(AttackTarget.transform.position);
            }
            else if (IsAttackCoroutineDone)
            {
                StopMoving();
                StartCoroutine(routine);
            }

            return !ias.IsAlive;
        }

        private IEnumerator MakeDamage(IAttackSusceptible ias)
        {
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsAttacking, true);
            }

            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsAttacking, false);
            }

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

        public virtual bool Move(Vector3 point)
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
                return true;
            }

            return false;
        }

        protected bool Rotate(Vector3 point)
        {
            var targetRotation = Quaternion.LookRotation(point - transform.position);
            var angles = targetRotation.eulerAngles;
            angles.y += 180;
            targetRotation.eulerAngles = angles;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

            if (Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 2)
            {
                return true;
            }

            return false;
        }

        public virtual void StopMoving()
        {
            NavMeshAgent.isStopped = true;
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.Walking, false);
            }
        }

        public virtual bool Select()
        {
            Selected = true;
            UnitUi.SetActive(true);

            return true;
        }

        public virtual bool Deselect()
        {
            Selected = false;
            UnitUi.SetActive(false);

            return true;
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

        public virtual void Death()
        {
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsDead, true);
            }
        }

        public virtual bool Delete()
        {
            // Death();
            Destroy(gameObject, 5000f);

            return true;
        }
    }
}