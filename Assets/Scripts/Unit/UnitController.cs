using UnityEngine;
using UnityEngine.UI;

using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitController : UnitBase
    {
        public GameObject ObjectAttachedTo { get; set; }

        [SerializeField]
        private Text _speedText;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = _defaultMaterial;
        }

        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;

            if (_speedText != null)
            {
                _speedText.text = $"{_speed}";
            }
        }

        private void FixedUpdate()
        {
            if (_isMoving) Move();
            if (_isRotating) Rotate();
        }

        public override void HideUI()
        {
            _unitUI.SetActive(false);
        }

        public override void AskToMove(Vector3 point)
        {
            _pointToMove = point;
            _pointToRotate = point;

            _pointToMove.y = this.transform.position.y;

            _isMoving = true;
            _isRotating = true;

            if (_animator != null)
            {
                _animator.SetBool("IsRuning", _isMoving);
            }

            if (this.tag == "AttachedToMineUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    ObjectAttachedTo.GetComponent<MineEdgeController>().DeatachUnit();
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

        private void OnLeftClick(RaycastHit hit)
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                Select();
            }
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            if (gch != null)
                gch.GameObjectLeftClick -= OnLeftClick;
        }
    }
}
