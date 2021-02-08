using UnityEngine;

using System.Linq;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.UI;
using StrategyGame.Assets.Scripts.Building;

namespace StrategyGame.Assets.Scripts.Terrain
{
    public class TerrainClickHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buildingPanelUI;

        private UnitManager _unitManager;
        private BuildingsPanelManager _buildingsPanelManager;
        private BuildingManager _buildingManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _buildingManager = FindObjectOfType<BuildingManager>();
            _buildingsPanelManager = FindObjectOfType<BuildingsPanelManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;
            gch.RightMouseButtonUp += OnRightClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
        }

        private void OnRightClick(RaycastHit hit)
        {
            if (hit.transform.tag == this.tag && !_unitManager.SelectedUnits.Any() && !_buildingManager.SelectedBuildings.Any())
            {
                ManageEmptyRightClick();
            }
        }

        private void ManageEmptyRightClick()
        {
            if (_buildingPanelUI.activeSelf)
            {
                _buildingPanelUI.SetActive(false);
            }
            else
            {
                _buildingPanelUI.SetActive(true);
            }
        }
    }
}