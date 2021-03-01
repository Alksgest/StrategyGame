using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Commands;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Static;
using Assets.Scripts.Util;
using Assets.Scripts.WorldState;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitBase> _unitControllers;

        public IEnumerable<UnitBase> SelectedUnits => _unitControllers.Where(unit => unit.Selected);

        public IEnumerable<WorkerController> SelectedWorkers => SelectedUnits
            .OfType<WorkerController>();

        private bool _isSelecting;
        private GameManager _gameManager;
        private Vector3 _startMousePosition;

        private void OnGUI()
        {
            if (_isSelecting)
            {
                // Create a rect from both mouse positions
                var rect = RectangleUtil.GetScreenRect(_startMousePosition, Input.mousePosition);
                RectangleUtil.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                RectangleUtil.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
            }
        }

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            var units = Resources.FindObjectsOfTypeAll<UnitBase>();
            _unitControllers = new List<UnitBase>();

            foreach (var unitBase in units)
            {
                if (string.IsNullOrEmpty(unitBase.UnitId) ||
                    _unitControllers.SingleOrDefault(el => el.UnitId == unitBase.UnitId))
                {
                    continue;
                }

                _unitControllers.Add(unitBase);
            }

            var unitTemplates = StaticData.GetUnitTemplates();

            foreach (var unit in _unitControllers)
            {
                var t = unitTemplates.SingleOrDefault(el => el.UnitName == unit.tag);
                if (t != null)
                {
                    unit.Instantiate(t.UnitStats);
                }
            }

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;
            gch.RightMouseButtonUp += OnRightClick;
        }

        private void Update()
        {
            // If we press the left mouse button, save mouse location and begin selection
            if (Input.GetMouseButtonDown((int) MouseButton.LeftMouseButton) &&
                !Input.GetMouseButtonDown((int) MouseButton.RightMouseButton))
            {
                _isSelecting = true;
                _startMousePosition = Input.mousePosition;
            }

            // If we let go of the left mouse button, end selection
            if (Input.GetMouseButtonUp((int) MouseButton.LeftMouseButton))
            {
                SelectAllUnitsInArea();
                _isSelecting = false;
            }
        }

        private void CheckUnitsAndDelete()
        {
            var unitsToDelete = _unitControllers.Where(u => !u.IsAlive).ToList();

            _unitControllers = _unitControllers.Except(unitsToDelete).ToList();

            foreach (var unit in unitsToDelete)
            {
                unit.Execute(new DeleteCommand<UnitBase>());
            }
        }

        private void OnLeftClick(RaycastHit hit)
        {
            var isWorkerBusy = SelectedWorkers.Any(el => el.IsSettingBuilding);

            //var unit = hit.transform.gameObject.GetComponent<UnitBase>();
            var unit = hit.transform.gameObject.GetComponent<UnitBase>();
            if (unit != null)
            {
                unit.Execute(new SelectCommand<UnitBase>());

                if (SelectedUnits.Count() > 1)
                {
                    HideUnitsUi();
                }
            }
            else if (!isWorkerBusy)
            {
                DeselectAll();
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            switch (hit.transform.root.tag)
            {
                case "BuildingManager":
                    HandleBuildingClick(hit.transform.gameObject);
                    break;
                case "UnitManager":
                    AttackObject(hit.transform.gameObject);
                    break;
                default:
                    MoveUnitsToPoint(hit.point);
                    break;
            }
        }

        private void HandleBuildingClick(GameObject obj)
        {
            var building = FindHelper.GetOfType<IBuildable>(obj);
            if (building != null && building.BuildingProgress < 100)
            {
                SendToBuild(building);
                return;
            }

            var wp = FindHelper.GetOfType<IWorkplace>(obj);
            if (wp != null)
            {
                SendToWork(wp);
                return;
            }
        }

        private void SendToBuild(IBuildable building)
        {
            foreach (var unit in SelectedWorkers)
            {
                //unit.Execute(
                //    new MoveCommand<UnitBase>(
                //        CalculationHelper.GetCorrectDestination(building.Destination, unit.transform.position, 0.2f)));
                unit.Execute(
                    new MoveCommand<UnitBase>(building.Destination - Vector3.one + new Vector3(0, 1, 0))); // TODO: replace with more beautiful code
                unit.Execute(new BuildCommand<UnitBase>(building, false));
            }
        }

        private void AttachUnit(GameObject obj)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Execute(new AttachCommand<UnitBase>(obj));
            }
        }

        private void SendToWork(IWorkplace workplace)
        {
            foreach (var unit in SelectedWorkers)
            {
                var position = workplace.GetFreePosition();
                if (position != null)
                {
                    unit.Execute(new MoveCommand<UnitBase>(position.GetValueOrDefault()));
                    unit.Execute(new AttachToWorkCommand<UnitBase>(workplace));
                }
            }
        }

        private void AttackObject(GameObject obj)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Execute(new AttackCommand<UnitBase>(obj));
            }
        }

        private void MoveUnitsToPoint(Vector3 point)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Execute(new MoveCommand<UnitBase>(point));
            }
        }

        private void DeselectAll()
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Execute(new DeselectCommand<UnitBase>());
            }
        }

        public void CreateUnit(UnitTemplate template, Vector3 unitPosition)
        {
            if (!_gameManager.CanBuyUnit("mainPlayer", template.UnitName)) return;

            var unit = _gameManager.BuyUnit("mainPlayer", template, unitPosition, transform);

            var unitController = unit.GetComponent<UnitBase>();
            unitController.Instantiate(template.UnitStats);

            _unitControllers.Add(unit.GetComponent<UnitBase>());
        }

        private void HideUnitsUi()
        {
            if (SelectedUnits.Count() <= 1) return;

            foreach (var u in SelectedUnits)
            {
                u.HideUi();
            }
        }

        private void SelectAllUnitsInArea()
        {
            if (!_isSelecting)
                return;

            var camera = UnityEngine.Camera.main;
            var viewportBounds =
                RectangleUtil.GetViewportBounds(UnityEngine.Camera.main, _startMousePosition, Input.mousePosition);

            foreach (var unit in _unitControllers)
            {
                var unitPosition = camera.WorldToViewportPoint(unit.transform.position);
                if (viewportBounds.Contains(unitPosition))
                {
                    unit.Execute(new SelectCommand<UnitBase>());
                }
            }
        }
    }
}