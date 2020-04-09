using UnityEngine;

using StrategyGame.Assets.Scripts.Building;

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

        public GameObject ObjectAttachedTo { get; set; }

        private bool _canMove = false;

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

            _pointToMove.y = this.transform.position.y;

            _isMoving = true;

            if (this.tag == "AttachedToMineUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    ObjectAttachedTo.GetComponent<MineEdgeController>().DeatachUnit();
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
                    _isMoving = false;
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