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
            var buildings = FindObjectsOfType<BuildingBase>(); // !!!!! TODO: remove from this list if delete building
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
            if (hit.transform.gameObject.tag != "BuildingBlock" && hit.transform.gameObject.tag != "MineEdge")
            {
                DeselectAll();
                return;
            }

            BuildingBase building = GetBuilding(hit.transform.gameObject);

            if (building != null)
            {
                building.RightClick(hit);
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
            if (hit.transform.gameObject.tag != "BuildingBlock" && hit.transform.gameObject.tag != "MineEdge")
            {
                DeselectAll();
                return;
            }

            DeselectAll();

            BuildingBase building = GetBuilding(hit.transform.gameObject);

            if (building != null)
            {
                building.LeftClick(hit);
            }
        }

        private static BuildingBase GetBuilding(GameObject gameObject)
        {
            var bb = gameObject.GetComponent<BuildingBase>();
            if (bb == null)
            {
                if (gameObject.transform.parent == null)
                {
                    return null;
                }
                bb = GetBuilding(gameObject.transform.parent.gameObject);
            }

            return bb;
        }

    }
}