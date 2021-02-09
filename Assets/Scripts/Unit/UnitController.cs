using UnityEngine;
using UnityEngine.UI;

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

            _previousStats = _currentStats;
        }

        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;

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
