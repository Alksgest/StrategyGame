using System.Collections.Generic;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Static;
using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.UI
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
            var template = _unitTemplates.Find(el => el.UnitName == tag);

            _unitManager.CreateUnit(template, _spawnPoint.position);
        }
    }
}