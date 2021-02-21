using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commands;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Building
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

        private void OnLeftClick(RaycastHit hit)
        {
            if (hit.transform.gameObject.tag != "BuildingBlock" && hit.transform.gameObject.tag != "MineEdge")
            {
                DeselectAll();
                return;
            }

            DeselectAll();

            var building = FindHelper.GetOfType<BuildingBase>(hit.transform.gameObject);

            if (building != null)
            {
                building.Execute(new SelectCommand<BuildingBase>());
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            if (hit.transform.gameObject.tag != "BuildingBlock" && hit.transform.gameObject.tag != "MineEdge")
            {
                DeselectAll();
                return;
            }

            var building = FindHelper.GetOfType<BuildingBase>(hit.transform.gameObject);

            if (building != null)
            {
                building.Execute(new DeselectCommand<BuildingBase>());
            }
        }

        public void DeselectAll()
        {
            foreach (var building in _buildings.Where(el => el.Selected))
            {
                building.Execute(new DeselectCommand<BuildingBase>());
            }
        }
    }
}