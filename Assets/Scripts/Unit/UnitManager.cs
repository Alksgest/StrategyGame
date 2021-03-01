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
        private Vector3 _startSelectionMousePosition;

        private void OnGUI()
        {
            if (!_isSelecting) return;

            var rect = RectangleUtil.GetScreenRect(_startSelectionMousePosition, Input.mousePosition);
            RectangleUtil.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            RectangleUtil.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            var units = Resources.FindObjectsOfTypeAll<UnitBase>();
            _unitControllers = new List<UnitBase>();

            var unitTemplates = StaticData.GetUnitTemplates();

            foreach (var unitBase in units)
            {
                if (string.IsNullOrEmpty(unitBase.UnitId) ||
                    _unitControllers.SingleOrDefault(el => el.UnitId == unitBase.UnitId))
                {
                    continue;
                }

                var t = unitTemplates.SingleOrDefault(el => el.UnitName == unitBase.tag);
                unitBase.Instantiate(t.UnitStats);

                _unitControllers.Add(unitBase);
            }

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftMouseButtonUp;
            gch.RightMouseButtonUp += OnRightMouseButtonUp;
            gch.LeftMouseButtonDown += OnLeftClickDown;
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

        private void OnLeftClickDown(RaycastHit _)
        {
            if (!Input.GetMouseButton((int) MouseButton.RightMouseButton) &&
                !Input.GetMouseButtonDown((int) MouseButton.RightMouseButton) &&
                !Input.GetMouseButtonUp((int) MouseButton.RightMouseButton))
            {
                _isSelecting = true;
                _startSelectionMousePosition = Input.mousePosition;
            }
        }

        private void OnLeftMouseButtonUp(RaycastHit hit)
        {
            var isSelected = SelectAllUnitsInArea();
            _isSelecting = false;

            if (isSelected) return;

            var isWorkerBusy = SelectedWorkers.Any(el => el.IsSettingBuilding);

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

        private void OnRightMouseButtonUp(RaycastHit hit)
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
                unit.Execute(
                    new MoveCommand<UnitBase>(building.Destination -
                                              new Vector3(1f, 0f, 1f))); // TODO: replace with more beautiful code
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
            var units = SelectedUnits.ToList();
            var count = units.Count;

            var unitPositions = new List<Vector3>();

            var leftSide = Vector3.left * (count / 2f);
            var currentCol = 0;

            for (var i = 0; i < count; i++)
            {
                var fakeDest = point + leftSide + Vector3.right * count;
                fakeDest += Vector3.back * currentCol;

                unitPositions.Add(fakeDest);

                currentCol++;
            }

            for (var i = 0; i < units.Count; i++)
            {
                units[i].Execute(new MoveCommand<UnitBase>(unitPositions[i]));

                Debug.Log(unitPositions[i]);
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

        private bool SelectAllUnitsInArea()
        {
            if (!_isSelecting)
                return false;

            var isSelected = false;

            var camera = UnityEngine.Camera.main;
            var viewportBounds =
                RectangleUtil.GetViewportBounds(UnityEngine.Camera.main, _startSelectionMousePosition, Input.mousePosition);

            foreach (var unit in _unitControllers)
            {
                var unitPosition = camera.WorldToViewportPoint(unit.transform.position);
                if (viewportBounds.Contains(unitPosition))
                {
                    unit.Execute(new SelectCommand<UnitBase>());
                    isSelected = true;
                }
            }

            return isSelected;
        }
    }
}