using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitBase> _controllers;

        public List<UnitBase> SelectedUnits => _controllers.Where(unit => unit.Selected).ToList();

        public List<WokerController> SelectedWorkers => SelectedUnits
                                                        .Where(unit => unit is WokerController)
                                                        .Select(unit => unit as WokerController)
                                                        .ToList();

        private void Start()
        {
            var units = Resources.FindObjectsOfTypeAll(typeof(UnitBase));

            _controllers = new List<UnitBase>(units as UnitBase[]);

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
        }

        public void MoveUnitsToPoint(Vector3 point)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.AskToMove(point);
            }
        }

        public void DeselectAll()
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Select();
            }
        }

        public void CreateWorker(GameObject prefab, Vector3 creatorPosition)
        {
            var unit = GameObject.Instantiate(prefab, creatorPosition, new Quaternion(0, 0, 0, 0), this.transform);
            _controllers.Add(unit.GetComponent<WokerController>());
        }

        private void OnLeftClick(RaycastHit hit)
        {
            var unit = hit.transform.gameObject.GetComponent<UnitBase>();
            if (unit == null)
            {
                DeselectAll();
            }
            if (unit != null && SelectedUnits.Count > 1)
            {
                foreach (var u in SelectedUnits)
                {
                    u.HideUI();
                }
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            MoveUnitsToPoint(hit.point);
        }

    }
}