using UnityEngine;

using System.Linq;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.UI;

namespace StrategyGame.Assets.Scripts.Terrain
{
    public class TerrainClickHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buildingPanelUI;

        private UnitManager _unitManager;
        private BuildingsPanelManager _buildingsPanelManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _buildingsPanelManager = FindObjectOfType<BuildingsPanelManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            // if (hit.transform.tag == this.tag && !_unitManager.SelectedUnits.Any() && _buildingsPanelManager?.CanPlaceBuilding == false)
            // {
            //     _buildingPanelUI.SetActive(false);
            // }
        }

        private void OnRightClick(RaycastHit hit)
        {
            if (hit.transform.tag == this.tag && !_unitManager.SelectedUnits.Any())
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