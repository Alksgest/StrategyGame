using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Unit
{
    public abstract class UnitBase : MonoBehaviour, IEquatable<UnitBase>, ICommandExecutor<UnitBase>, IUnit
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

        public bool Selected { get; protected set; } = false;

        public abstract void HideUi();

        public void Execute(ICommand<UnitBase> command)
        {
            command.Execute(this);
        }

        public virtual void Move(Vector3 point)
        {
            SetAttackTarget(null);
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

        public virtual void SetAttackTarget(GameObject target)
        {
            AttackTarget = target;
        }

        public virtual void Instantiate(UnitStats stats)
        {
            CurrentStats = stats;
            PreviousStats = UnitStats.MakeCopy(stats);
        }

        protected virtual void Attack()
        {
            Move(AttackTarget.transform.position - new Vector3(CurrentStats.AttackRange, 0, CurrentStats.AttackRange));
        }

        public bool Equals(UnitBase other)
        {
            return UnitId == other?.UnitId;
        }

        public override int GetHashCode()
        {
            return UnitId.GetHashCode();
        }

        // protected virtual void Rotate()
        // {
        //     if (_pointToRotate != null)
        //     {
        //         var targetRotation = Quaternion.LookRotation(_pointToRotate - transform.position);
        //         var angles = targetRotation.eulerAngles;
        //         angles.y += 180;
        //         targetRotation.eulerAngles = angles;
        //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

        //         if (Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 2)
        //         {
        //             _isRotating = false;
        //         }
        //     }
        // }

        // protected virtual void Move()
        // {
        //     if (_canMove)
        //     {
        //         var delta = _pointToMove - transform.position;
        //         delta.Normalize();
        //         transform.position = transform.position + (delta * _speed * Time.deltaTime);

        //         var vec = this.transform.position - _pointToMove;
        //         if (Mathf.Abs(vec.x) <= 0.1 && Mathf.Abs(vec.z) <= 0.1)
        //         {
        //             _isMoving = false;
        //             if (_animator != null)
        //             {
        //                 _animator.SetBool("IsRuning", _isMoving);
        //             }
        //         }
        //     }
        // }
    }
}