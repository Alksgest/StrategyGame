using UnityEngine;

using StrategyGame.Assets.Scripts.UI;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Terrain
{
    public class TerrainClickHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buildingPanelUI;

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == this.tag)
            {
                var manager = _buildingPanelUI.GetComponent<BuildingsPanelManager>();
                if (manager.ObjectToCreate != null && manager.CanPlaceBuilding)
                {
                    manager.SetBuildingOnPlace();
                }
                else
                {
                    _unitManager.DeselectAll();
                }
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == this.tag)
            {
                if (_unitManager.SelectedUnits.Count > 0)
                {
                    _unitManager.MoveUnitsToPoint(hit.point);
                }
                else
                {
                    ManageBuildingPlacement();
                }
            }

        }

        private void ManageBuildingPlacement()
        {
            if (_buildingPanelUI.activeSelf)
            {
                var manager = _buildingPanelUI.GetComponent<BuildingsPanelManager>();
                manager.RemoveUnsettedBuilding();
                _buildingPanelUI.SetActive(false);
            }
            else
            {
                _buildingPanelUI.SetActive(true);
            }
        }
    }
}