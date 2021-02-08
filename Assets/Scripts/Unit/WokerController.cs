using UnityEngine;
using UnityEngine.UI;

using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class WokerController : UnitBase
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
            gch.LeftMouseButtonUp += OnLeftClick;

            if (_speedText != null)
            {
                _speedText.text = $"{_speed}";
            }
        }

        public override void Select()
        {
            Selected = !Selected;
            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = Selected ? _selectedMaterial : _defaultMaterial;
            _unitUI.SetActive(Selected);
        }

        public override void HideUI()
        {
            _unitUI.SetActive(false);
        }

        public override void AskToMove(Vector3 point)
        {
            _agent.SetDestination(point);

            if (_animator != null)
            {
                _animator.SetBool("IsRuning", true);
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
                gch.LeftMouseButtonUp -= OnLeftClick;
        }
    }
}
