using UnityEngine;

using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BarrackController : BuildingBase
    {
        [SerializeField]
        private GameObject _unitPrefab;
        [SerializeField]
        private GameObject _barrackUI;
        [SerializeField]
        private GameObject _spawnPoint;

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            _isInstantiated = false;
            gch.GameObjectLeftClick += OnLeftClick;
        }

        public void CreateNewUnit()
        {
            _barrackUI.SetActive(true);

            _unitManager.CreateWorker(_unitPrefab, _spawnPoint.transform.position);
        }

        private void OnLeftClick(RaycastHit hit)
        {
            if (_isInstantiated)
            {
                _barrackUI.SetActive(false);
                if (hit.transform.tag == this.tag && hit.transform.gameObject == this.gameObject)
                {
                    _barrackUI.SetActive(!_barrackUI.activeSelf);
                }
            }
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            _isInstantiated = false;
            if (gch != null)
                gch.GameObjectLeftClick -= OnLeftClick;
        }
    }
}