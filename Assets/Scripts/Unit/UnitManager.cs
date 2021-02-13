using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.WorldState;
using StrategyGame.Assets.Scripts.Models.Unit;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitBase> _unitControllers;

        public List<UnitBase> SelectedUnits => _unitControllers.Where(unit => unit.Selected).ToList();

        public List<WorkerController> SelectedWorkers => SelectedUnits
                                                        .Where(unit => unit is WorkerController)
                                                        .Select(unit => unit as WorkerController)
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

        private void DrowRect(Image image, Vector3 right)
        {

            image.rectTransform.sizeDelta =
                CalculationHelper.CalculateRectangleSize(_startHitPoint, right);
            image.transform.position =
                CalculationHelper.CalculateRectanglePosition(_selectCanvas.transform.position, image.rectTransform.sizeDelta);
        }

        private void CreateSelectCanvas(Vector3 position)
        {
            _selectCanvas = new GameObject();
            _selectCanvas.name = "SelectionCanvas";

            var canvas = _selectCanvas.AddComponent<Canvas>();

            _selectImage = new GameObject();
            _selectImage.transform.SetParent(canvas.transform);
            _selectImage.name = "SelectionRectangle";
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
                unit.Deselect();
            }
        }

        public void CreateUnit(UnitTemplate template, Vector3 unitPosition)
        {
            if (_gameManager.CanBuyUnit("mainPlayer", template.UnitName))
            {
                var unit  = _gameManager.BuyUnit("mainPlayer", template, unitPosition, this.transform);
                // var unit = GameObject.Instantiate(template.Prefab, unitPosition, new Quaternion(0, 0, 0, 0), this.transform);

                var unitController = unit.GetComponent<UnitBase>();
                unitController.Instantiate(template.UnitStats);

                _unitControllers.Add(unit.GetComponent<UnitBase>());
            }
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
                unit.Select();

                if (SelectedUnits.Count > 1)
                {
                    HideUnitsUI();
                }
            }
            else if (unit == null && !isWorkerBusy)
            {
                DeselectAll();
            }
        }

        private void HideUnitsUI()
        {
            if (SelectedUnits.Count > 1)
            {
                foreach (var u in SelectedUnits)
                {
                    u.HideUI();
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
                    HideUnitsUI();
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

        private void OnRightClick(RaycastHit hit)
        {
            if (hit.transform.root.tag != "BuildingManager")
            {
                MoveUnitsToPoint(hit.point);
            }
        }
    }
}