using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitController> _controllers;

        public List<UnitController> SelectedUnits => _controllers.Where(unit => unit.Selected).ToList();

        private void Start()
        {
            var units = Resources.FindObjectsOfTypeAll(typeof(UnitController));

            _controllers = new List<UnitController>(units as UnitController[]);

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

        public void CreateUnit(GameObject prefab, Vector3 creatorPosition)
        {
            var unit = GameObject.Instantiate(prefab, creatorPosition, new Quaternion(0, 0, 0, 0), this.transform);
            _controllers.Add(unit.GetComponent<UnitController>());
        }

        private void OnLeftClick(RaycastHit hit)
        {
            var unit = hit.transform.gameObject.GetComponent<UnitBase>();
            if (unit == null)
            {
                DeselectAll();
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            MoveUnitsToPoint(hit.point);
        }

    }
}