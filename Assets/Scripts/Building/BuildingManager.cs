using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BuildingManager : MonoBehaviour
    {
        private List<BuildingBase> _buildings;
        public IEnumerable<BuildingBase> SelectedBuildings => _buildings?.Where(el => el.Selected);

        private void Awake()
        {
            var buildings = FindObjectsOfType<BuildingBase>();
            _buildings = new List<BuildingBase>();
            foreach (var b in buildings)
            {
                b.Instantiate();

                _buildings.Add(b);
            }

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;
            gch.RightMouseButtonUp += OnRightClick;
        }

        public void AddBuilding(BuildingBase building)
        {
            _buildings.Add(building);
        }

        private void OnRightClick(RaycastHit hit)
        {
            BuildingBase building = GetBuilding(hit);

            if (building != null)
            {
                building.RightClick(hit);
            }
            else
            {
                DeselectAll();
            }
        }

        public void DeselectAll()
        {
            foreach (var s in _buildings.Where(el => el.Selected))
            {
                s.Deselect();
            }
        }

        private void OnLeftClick(RaycastHit hit)
        {
            BuildingBase building = GetBuilding(hit);

            if (building != null)
            {
                building.LeftClick(hit);
            }
            else
            {
                DeselectAll();
            }
        }

        private static BuildingBase GetBuilding(RaycastHit hit)
        {
            BuildingBase building;

            building = hit.transform.parent.gameObject.GetComponent<BuildingBase>();

            // if (hit.transform.tag == "MineEdge")
            // {
            //     building = hit.transform.parent.gameObject.GetComponent<BuildingBase>();
            // }
            // else
            // {
            //     building = hit.transform.gameObject.GetComponent<BuildingBase>();
            // }

            return building;
        }
    }
}