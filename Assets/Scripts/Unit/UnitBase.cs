using UnityEngine;
using UnityEngine.AI;

namespace StrategyGame.Assets.Scripts.Unit
{
    public abstract class UnitBase : MonoBehaviour
    {
        [SerializeField]
        protected float _speed = 0.01f;

        [SerializeField]
        protected Material _defaultMaterial;
        [SerializeField]
        protected Material _selectedMaterial;

        [SerializeField]
        protected GameObject _unitUI;

        protected bool _canMove = false;
        protected Animator _animator;

        public bool Selected { get; protected set; } = false;

        public abstract void HideUI();
        public abstract void AskToMove(Vector3 point);

        [SerializeField]
        protected NavMeshAgent _agent;

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