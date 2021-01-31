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

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
        }

        public void CreateNewUnit()
        {
            _barrackUI.SetActive(true);

            _unitManager.CreateUnit(_unitPrefab, this.transform.position + new Vector3(0, 0, 10));
        }


        private void OnLeftClick(RaycastHit hit)
        {
            if (_isInstantiated)
            {
                _barrackUI.SetActive(false);

                if (hit.transform.parent.tag == this.tag && hit.transform.parent.gameObject == this.gameObject)
                {
                    _barrackUI.SetActive(!_barrackUI.activeSelf);
                }
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