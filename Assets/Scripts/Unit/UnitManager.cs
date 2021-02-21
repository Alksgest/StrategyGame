using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commands;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Static;
using Assets.Scripts.Util;
using Assets.Scripts.WorldState;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitBase> _unitControllers;

        public List<UnitBase> SelectedUnits => _unitControllers.Where(unit => unit.Selected).ToList();

        public List<WorkerController> SelectedWorkers => SelectedUnits
            .OfType<WorkerController>()
            .ToList();

        private bool _isLeftMouseHold = false;
        private GameObject _selectCanvas = null;
        private GameObject _selectImage = null;
        private Vector3 _startHitPoint;
        private bool _rightMousePressed = false;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            var units = Resources.FindObjectsOfTypeAll<UnitBase>();

            _unitControllers = units.Distinct().ToList();

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
            gch.LeftMouseButtonHold += OnLeftMouseKeyHold;
            gch.RightMouseButtonDown += OnRightMouseButtonDown;
            gch.RightMouseButtonUp += OnRightMouseButtonUp;
        }

        private void OnRightMouseButtonUp(RaycastHit hit)
        {
            _rightMousePressed = false;
        }

        private void OnRightMouseButtonDown(RaycastHit hit)
        {
            _rightMousePressed = true;
        }

        private void FixedUpdate()
        {
            // if (_isLeftMouseHold && !_rightMousePressed && !SelectedWorkers.Any(el => el.IsBuilding))
            // {
            //     var mouse = Input.mousePosition;
            //     var castPoint = Camera.main.ScreenPointToRay(mouse);

            //     if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
            //     {
            //         if (_selectCanvas == null)
            //         {
            //             _startHitPoint = hit.point;
            //             CreateSelectCanvas(_startHitPoint);
            //         }
            //         var image = _selectImage.GetComponent<Image>();

            //         DrowRect(image, hit.point);
            //     }
            // }
        }

        private void DrawRect(Image image, Vector3 right)
        {
            image.rectTransform.sizeDelta =
                CalculationHelper.CalculateRectangleSize(_startHitPoint, right);
            image.transform.position =
                CalculationHelper.CalculateRectanglePosition(_selectCanvas.transform.position, image.rectTransform.sizeDelta);
        }

        private void CreateSelectCanvas(Vector3 position)
        {
            _selectCanvas = new GameObject {name = "SelectionCanvas"};

            var canvas = _selectCanvas.AddComponent<Canvas>();

            _selectImage = new GameObject {name = "SelectionRectangle"};
            _selectImage.transform.SetParent(canvas.transform);
            _selectImage.transform.Rotate(new Vector3(90, 0, 0));

            var image = _selectImage.AddComponent<Image>();
            image.color = new Color(1.0F, 0.0F, 0.0F, 0.2f);

            var deltaX = image.rectTransform.sizeDelta.x / 2 * -1;
            var deltaZ = image.rectTransform.sizeDelta.y / 2;

            image.transform.position = new Vector3(deltaX, 0, deltaZ);
            canvas.transform.position = new Vector3(position.x, 5, position.z);
        }

        private void OnLeftMouseKeyHold(RaycastHit hit)
        {
            _isLeftMouseHold = true;
        }

        private void OnRightClick(RaycastHit hit)
        {
            switch (hit.transform.root.tag)
            {
                case "BuildingManager":
                    break;
                case "UnitManager":
                    AttackObject(hit.transform.gameObject);
                    break;
                default:
                    MoveUnitsToPoint(hit.point);
                    break;
            }
        }

        public void AttackObject(GameObject obj)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.SetAttackTarget(obj);
            }
        }

        public void MoveUnitsToPoint(Vector3 point)
        {
            foreach (var unit in SelectedUnits)
            {
                unit.Execute(new MoveCommand<UnitBase>(point));
            }
        }

        public void DeselectAll()
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

        private void OnLeftClick(RaycastHit hit)
        {
            var isWorkerBusy = SelectedWorkers.Any(el => el.IsBuilding);

            // if (_selectCanvas != null && !isWorkerBusy)
            // {
            //     DeselectAll();
            //     FinalizeSelection();
            // }

            var unit = hit.transform.gameObject.GetComponent<UnitBase>();
            if (unit != null)
            {
                unit.Execute(new SelectCommand<UnitBase>());

                if (SelectedUnits.Count > 1)
                {
                    HideUnitsUi();
                }
            }
            else if (!isWorkerBusy)
            {
                DeselectAll();
            }
        }

        private void HideUnitsUi()
        {
            if (SelectedUnits.Count > 1)
            {
                foreach (var u in SelectedUnits)
                {
                    u.HideUi();
                }
            }
        }

        private void SelectAllUnitsInArea()
        {
            if (_selectImage != null)
            {
                var image = _selectImage.GetComponent<Image>();

                var selectorPosition = image.transform.position;
                var selectorSize = image.rectTransform.sizeDelta;

                foreach (var unit in _unitControllers) //TODO: rewrite this with smth more quick
                {
                    if (CalculationHelper.IsInArea(unit.transform.position, selectorPosition, selectorSize))
                    {
                        unit.Select();
                    }
                }
                if (SelectedUnits.Count > 1)
                {
                    HideUnitsUi();
                }
            }
        }

        private void FinalizeSelection()
        {
            SelectAllUnitsInArea();
            DestroyCanvas();

            _isLeftMouseHold = false;
        }

        private void DestroyCanvas()
        {
            _startHitPoint = Vector3.zero;
            Destroy(_selectCanvas);
            _selectCanvas = null;
            _selectImage = null;
        }
    }
}