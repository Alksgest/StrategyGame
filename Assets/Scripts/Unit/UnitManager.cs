using UnityEngine;

using System.Linq;
using System.Collections.Generic;

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

    }
}