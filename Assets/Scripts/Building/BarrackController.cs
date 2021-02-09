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
        private GameObject _spawnPoint;

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            _isInstantiated = false;
        }

        public void CreateNewUnit()
        {
            _unitManager.CreateWorker(_unitPrefab, _spawnPoint.transform.position);
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            _isInstantiated = false;
        }

        public override void LeftClick(object obj)
        {
            if (_isInstantiated)
            {
                if (obj is RaycastHit hit)
                {
                    // if (hit.transform.tag == this.tag && hit.transform.gameObject == this.gameObject)
                    if (hit.transform.parent.gameObject == this.gameObject)
                    {
                        base.LeftClick(hit);
                    }
                }
            }
        }

        public override void RightClick(object obj)
        {
            base.RightClick(obj);
        }
    }
}