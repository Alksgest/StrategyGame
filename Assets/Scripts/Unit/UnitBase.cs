using System;
using StrategyGame.Assets.Scripts.Models.Unit;
using UnityEngine;
using UnityEngine.AI;

namespace StrategyGame.Assets.Scripts.Unit
{

    public abstract class UnitBase : MonoBehaviour, IEquatable<UnitBase>
    {

        [SerializeField]
        protected string _unitId;
        public string UnitId => _unitId;

        [SerializeField]
        protected Material _defaultMaterial;
        [SerializeField]
        protected Material _selectedMaterial;

        [SerializeField]
        protected GameObject _unitUI;
        protected Animator _animator;

        protected GameObject _attackTarget;

        public bool Selected { get; protected set; } = false;

        [SerializeField]
        protected NavMeshAgent _navMeshAgent;

        [SerializeField]
        protected UnitStats _currentStats;
        protected UnitStats _previousStats;

        public abstract void HideUI();

        public virtual void AskToMove(Vector3 point)
        {
            SetAttackTarget(null);
            Move(point);
        }
        public virtual void Move(Vector3 point)
        {
            _navMeshAgent.SetDestination(point);
        }

        public virtual void SetAttackTarget(GameObject target)
        {
            _attackTarget = target;
        }

        public virtual void Instantiate(UnitStats stats)
        {
            _currentStats = stats;
            _previousStats = UnitStats.MakeCopy(stats);
        }

        public virtual void Select()
        {
            if (!Selected)
            {
                Selected = true;
                var renderer = GetComponentInChildren<MeshRenderer>();
                renderer.material = _selectedMaterial;
                _unitUI.SetActive(true);
            }
        }

        public virtual void Deselect()
        {
            if (Selected)
            {
                Selected = false;
                var renderer = GetComponentInChildren<MeshRenderer>();
                renderer.material = _defaultMaterial;
                _unitUI.SetActive(false);
            }
        }

        protected virtual void Attack()
        {
            Move(_attackTarget.transform.position - new Vector3(_currentStats.AttackRange, 0, _currentStats.AttackRange));
        }

        public bool Equals(UnitBase other)
        {
            return this._unitId == other.UnitId;
        }

        public override int GetHashCode()
        {
            return this._unitId.GetHashCode();
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