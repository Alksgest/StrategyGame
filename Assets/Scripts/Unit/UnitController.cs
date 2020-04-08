using System;
using UnityEngine;

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

        private bool _isMoving = false;
        private Vector3 _pointToMove;

        private void FixedUpdate()
        {
            if (_isMoving) Move();
        }

        public void Select()
        {
            Selected = !Selected;

            var renderer = GetComponent<MeshRenderer>();
            renderer.material = Selected ? _selectedMaterial : _defaultMaterial;
        }

        public void AskToMove(Vector3 point)
        {
            Debug.Log(point);
            _pointToMove = point;
            _isMoving = true;
        }

        private void Move()
        {
            var delta = _pointToMove - transform.position;
            delta.Normalize();
            transform.position = transform.position + (delta * _speed * Time.deltaTime);

            if (this.transform.position == _pointToMove)
                _isMoving = false;
        }
    }
}