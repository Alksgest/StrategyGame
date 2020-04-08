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

        private void FixedUpdate()
        {
            if (_buildingPanelUI.GetComponent<BuildingsPanelManager>().ObjectToCreate != null)
            {
                var x = Input.GetAxis("Mouse X") * 3;
                var z = Input.GetAxis("Mouse Y") * 3;

                var created = _buildingPanelUI.GetComponent<BuildingsPanelManager>().ObjectToCreate;

                created.transform.position += new Vector3(-x, 0, -z);
            }
        }

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
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
            if (hit.transform.tag == this.tag)
            {
                if (_unitManager.SelectedUnits.Count > 0)
                    _unitManager.MoveUnitsToPoint(hit.point);
                else
                {
                    _buildingPanelUI.SetActive(!_buildingPanelUI.activeSelf);
                }
            }

        }
    }
}