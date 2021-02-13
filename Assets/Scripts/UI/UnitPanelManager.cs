using UnityEngine;

using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Models.Unit;
using StrategyGame.Assets.Scripts.Static;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.WorldState;

namespace StrategyGame.Assets.Scripts.UI
{
    public class UnitPanelManager : MonoBehaviour // TODO: could be abstract base for other
    {
        [SerializeField]
        private Transform _spawnPoint;

        private UnitManager _unitManager;
        private List<UnitTemplate> _unitTemplates;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            _unitTemplates = StaticData.GetUnitTemplates();
        }

        public void CreateNewUnit(string tag)
        {
            var prefab = _unitTemplates.Find(el => el.UnitName == tag).Prefab;

            _unitManager.CreateUnit(tag, prefab, _spawnPoint.position);
        }
    }
}