using UnityEngine;

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

        protected bool _canMove = false;
        protected bool _isRotating = true;

        protected bool _isMoving = false;
        protected Vector3 _pointToMove;
        protected Vector3 _pointToRotate;

        protected Animator _animator;

        public bool Selected { get; protected set; } = false;

        public abstract void HideUI();
        public abstract void AskToMove(Vector3 point);
        public abstract void Select();

        protected virtual void Rotate()
        {
            if (_pointToRotate != null)
            {
                var targetRotation = Quaternion.LookRotation(_pointToRotate - transform.position);
                var angles = targetRotation.eulerAngles;
                angles.y += 180;
                targetRotation.eulerAngles = angles;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

                if (Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 2)
                {
                    _isRotating = false;
                }
            }
        }

        protected virtual void Move()
        {
            if (_canMove)
            {
                var delta = _pointToMove - transform.position;
                delta.Normalize();
                transform.position = transform.position + (delta * _speed * Time.deltaTime);

                var vec = this.transform.position - _pointToMove;
                if (Mathf.Abs(vec.x) <= 0.1 && Mathf.Abs(vec.z) <= 0.1)
                {
                    _isMoving = false;
                    if (_animator != null)
                    {
                        _animator.SetBool("IsRuning", _isMoving);
                    }
                }
            }
        }
    }
}