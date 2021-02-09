using UnityEngine;
using UnityEngine.UI;

using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.UI;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class WorkerController : UnitBase
    {
        public GameObject ObjectAttachedTo { get; set; }

        [SerializeField]
        private BuildingsPanelManager _buildingsPanelManager;

        [SerializeField]
        private Text _speedText;

        public bool IsBuilding { get; set; }

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
            if (!Selected)
            {
                _buildingsPanelManager.gameObject.SetActive(true);
                base.Select();
            }
        }
        public override void Deselect()
        {
            if (Selected)
            {
                _buildingsPanelManager.gameObject.SetActive(false);
                base.Deselect();
            }
        }

        public override void HideUI()
        {
            _unitUI.SetActive(false);
            _buildingsPanelManager.gameObject.SetActive(false);
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
                    ObjectAttachedTo.GetComponent<MineController>().DeatachUnit(this);
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
            if (hit.transform.gameObject == this.gameObject && !_buildingsPanelManager.IsBuildSelected)
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
