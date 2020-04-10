using UnityEngine;

using StrategyGame.Assets.Scripts.Building;
using System;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        public bool Selected { get; private set; } = false;

        [SerializeField]
        private float _speed = 0.01f;

        [SerializeField]
        private Material _defaultMaterial;
        [SerializeField]
        private Material _selectedMaterial;

        private Animator _animator;

        private bool _isMoving = false;
        private Vector3 _pointToMove;
        private Vector3 _pointToRotate;

        public GameObject ObjectAttachedTo { get; set; }

        private bool _canMove = false;
        private bool _isRotating = true;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (_isMoving) Move();
            if (_isRotating) Rotate();
        }

        public void Select()
        {
            Selected = !Selected;
            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = Selected ? _selectedMaterial : _defaultMaterial;
        }

        public void AskToMove(Vector3 point)
        {
            // Debug.Log(point);
            // Debug.Log(this.transform.position);

            _pointToMove = point;
            _pointToRotate = point;

            _pointToMove.y = this.transform.position.y;

            _isMoving = true;
            _isRotating = true;

            _animator.SetBool("IsRuning", _isMoving);

            if (this.tag == "AttachedToMineUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    ObjectAttachedTo.GetComponent<MineEdgeController>().DeatachUnit();
                }
            }
        }

        private void Rotate()
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

        private void Move()
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
                    _animator.SetBool("IsRuning", _isMoving);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Terrain" && !_canMove)
            {
                _canMove = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.tag == "Terrain" && _canMove)
            {
                _canMove = false;
            }
        }
    }
}
