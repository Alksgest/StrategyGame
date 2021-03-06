using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Animation;
using Assets.Scripts.Models.Unit;
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
        [SerializeField] protected GameObject SelectionCircle;

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
            if (!IsAlive) return;

            if (command is IRejectableCommand<UnitBase> r)
            {
                if (r.Interrupt && RejectableCommandsQueue.Any())
                {
                    RejectLastCommand();
                }

                Debug.Log($"Added: {command.GetType().Name}");
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
                    Debug.Log($"Removed: {command.GetType().Name}");
                    RejectLastCommand();
                }
            }
        }

        protected virtual void RejectLastCommand()
        {
            var command = RejectableCommandsQueue.Dequeue();
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

            //var newDirection = Vector3.RotateTowards(transform.forward, target.transform.position, 5, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDirection);

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

            IsAttackCoroutineDone = false;
            yield return new WaitForSeconds(CurrentStats.AttackSpeed / 2);
            ias.TakeDamage(CurrentStats.Attack);
            yield return new WaitForSeconds(CurrentStats.AttackSpeed / 2);
            IsAttackCoroutineDone = true;

            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsAttacking, false);
            }

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

        [SerializeField] private float degreeStep = 45f;
        [SerializeField] private float followSpeed = 2f;

        public bool Rotate(Vector3 point)
        {
            var targetRotation = Quaternion.LookRotation(point - transform.position);

            var selfY = transform.rotation.y;
            var targetY = targetRotation.y;

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

            Debug.Log(selfY);
            Debug.Log(targetY);

            if (Math.Abs(selfY - targetY) < 0.1)
            {
                return true;
            }

            //transform.LookAt(point);

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

            SelectionCircle.SetActive(true);

            return true;
        }

        public virtual bool Deselect()
        {
            Selected = false;
            UnitUi.SetActive(false);

            SelectionCircle.SetActive(false);

            return true;
        }

        public virtual void TakeDamage(float value)
        {
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsTackingDamage, true);
            }

            StartCoroutine(StopTackingDamageAnimation());

            var damage = (value - CurrentStats.Armor) / CurrentStats.Defence;
            PreviousStats = UnitStats.MakeCopy(CurrentStats);
            CurrentStats.Health -= damage;
        }

        private IEnumerator StopTackingDamageAnimation()
        {
            yield return new WaitForSeconds(1);
            if (Animator != null)
            {
                Animator.SetBool(AnimationKind.IsTackingDamage, false);
            }
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

            if (RejectableCommandsQueue.Any())
            {
                foreach (var command in RejectableCommandsQueue)
                {
                    command.Reject(this);
                }

                CancelInvoke(nameof(ExecuteLastRejectableCommand));
            }

            NavMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            Deselect();
        }

        public virtual bool Delete()
        {
            // Death();
            Destroy(gameObject, 5000f);

            return true;
        }
    }
}