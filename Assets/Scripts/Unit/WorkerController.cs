using System;

using UnityEngine;
using UnityEngine.UI;

using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.UI;
using UnityEngine.AI;
using StrategyGame.Assets.Scripts.Unit.Interfaces;
using StrategyGame.Assets.Scripts.Building.Interfaces;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class WorkerController : UnitBase, IWorkable
    {
        public GameObject ObjectAttachedTo { get; set; }

        [SerializeField]
        private BuildingsPanelManager _buildingsPanelManager;

        [SerializeField]
        private Text _speedText;

        public bool IsBuilding { get; set; }
        public GameObject GameObject => this.gameObject;

        private void Start()
        {
            _unitId = Guid.NewGuid().ToString();

            _animator = GetComponent<Animator>();

            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = _defaultMaterial;
        }

        private void Awake()
        {
            if (_navMeshAgent == null)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>();
            }

            if (_speedText != null)
            {
                _speedText.text = $"{_currentStats.Speed}";
            }
        }

        private void FixedUpdate()
        {
            if (_currentStats != _previousStats)
            {
                _previousStats = UnitStats.MakeCopy(_currentStats);

                _speedText.text = $"{_currentStats.Speed}";
                _navMeshAgent.speed = _currentStats.Speed;
            }

            if (this.tag == "AttachedUnit")
            {
                if (_animator != null && !_animator.GetBool("IsMining")) // TOOD: rewrite this piece
                {
                    _animator.SetBool("IsMining", true);
                }
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
            _navMeshAgent.SetDestination(point);

            // if (_animator != null)
            // {
            //     _animator.SetBool("IsRuning", true);
            // }

            if (this.tag == "AttachedUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    var workplace = ObjectAttachedTo.GetComponent<IWorkplace>();
                    workplace.DeatachUnit(this);
                }

                // if (_animator != null)
                // {
                //     _animator.SetBool("IsMining", false);
                // }
            }
        }

        public void SetTag(string tag)
        {
            this.tag = tag;
        }
    }
}
