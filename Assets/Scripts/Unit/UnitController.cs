using System;

using UnityEngine;
using UnityEngine.UI;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitController : UnitBase
    {
        public GameObject ObjectAttachedTo { get; set; }

        [SerializeField]
        private Text _speedText;

        private void Start()
        {
            _unitId = Guid.NewGuid().ToString();

            _animator = GetComponent<Animator>();

            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = _defaultMaterial;

            _previousStats = _currentStats;
        }

        private void Awake()
        {
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
        }

        public override void HideUI()
        {
            _unitUI.SetActive(false);
        }

        public override void AskToMove(Vector3 point)
        {
            _navMeshAgent.SetDestination(point);

            if (_animator != null)
            {
                // _animator.SetBool("IsRuning", true);
            }
        }
    }
}
