using UnityEngine;


using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.Static;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BarrackController : BuildingBase
    {
        [SerializeField]
        private GameObject _barracksUI;

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            _isInstantiated = false;
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            _isInstantiated = false;
        }

        public override void Select()
        {
            base.Select();
            _barracksUI.SetActive(true);
        }

        public override void Deselect()
        {
            base.Deselect();
            _barracksUI.SetActive(false);
        }

        public override void LeftClick(object obj)
        {
            if (_isInstantiated)
            {
                base.LeftClick(obj);
            }
        }

        public override void RightClick(object obj)
        {
            base.RightClick(obj);
        }
    }
}