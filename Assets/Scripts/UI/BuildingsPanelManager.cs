using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Building;
using Assets.Scripts.Models.Building;
using Assets.Scripts.Static;
using Assets.Scripts.Unit;
using Assets.Scripts.Util;
using Assets.Scripts.WorldState;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

namespace Assets.Scripts.UI
{
    public class BuildingsPanelManager : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        [SerializeField] private GameObject _buildingPanelUi = null;

        public bool IsBuildSelected { get; private set; } = false;

        public GameObject ObjectToCreate { get; private set; }

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private GameManager _gameManager;
        private BuildingManager _buildingManager;
        private UnitManager _unitManager;
        private NavMeshSurface _surface;

        [SerializeField] private WorkerController _worker;

        private List<BuildingTemplate> _buildings;


        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;
            gch.RightMouseButtonUp += OnRightClick;

            _gameManager = FindObjectOfType<GameManager>();
            _buildingManager = FindObjectOfType<BuildingManager>();
            _unitManager = FindObjectOfType<UnitManager>();
            _surface = FindObjectOfType<NavMeshSurface>();

            _buildings = StaticData.GetBuildingTemplates();

            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateBuilding();
            }
        }

        private void FixedUpdate()
        {
            if (ObjectToCreate != null)
            {
                var mouse = Input.mousePosition;
                var castPoint = UnityEngine.Camera.main.ScreenPointToRay(mouse);

                if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
                {
                    var v = new Vector3(hit.point.x, ObjectToCreate.transform.position.y, hit.point.z);
                    ObjectToCreate.transform.position = v;
                }
            }
        }

        private void RotateBuilding()
        {
            ObjectToCreate.transform.Rotate(new Vector3(0, ObjectToCreate.transform.rotation.y + 20, 0));
        }

        public void CreateBuilding(GameObject prefab, float y)
        {
            var x = Input.GetAxis("Mouse X");
            var z = Input.GetAxis("Mouse Y");

            ObjectToCreate = Instantiate(prefab, new Vector3(x, y, z), new Quaternion(), _buildingManager.transform);
            ObjectToCreate.layer = LayerMask.NameToLayer("Terrain");

            //foreach (var child in transform.GetComponentsInChildren<BoxCollider>())
            //{
            //    child.isTrigger = true;
            //}

            _worker.IsSettingBuilding = true;

            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void SetBuildingOnPlace(BuildingBase building)
        {
            building.Instantiate();
            ObjectToCreate.layer = LayerMask.NameToLayer("Terrain");
            ObjectToCreate = null;
            IsBuildSelected = false;
            _buildingManager.AddBuilding(building);
            _worker.IsSettingBuilding = false;
        }

        public void RemoveUnsettedBuilding()
        {
            if (ObjectToCreate != null)
            {
                Destroy(ObjectToCreate.gameObject);
                ObjectToCreate = null;
                IsBuildSelected = false;
                _worker.IsSettingBuilding = false;
            }
        }

        private IEnumerator WaitForPlaceBuilding(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            IsBuildSelected = true;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            if (ObjectToCreate != null && IsBuildSelected)
            {
                var building = ObjectToCreate.GetComponent<BuildingBase>();
                var buildingTag = building.tag;
                if (
                    building.CanBePlaced &&
                    _gameManager.CanPlaceBuilding("mainPlayer", buildingTag))
                {
                    SetBuildingOnPlace(building);
                    _gameManager.BuyBuilding("mainPlayer", buildingTag);

                    _surface.BuildNavMesh();
                }
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            if (this == null ||this.gameObject == null)
            {
                return;
            }

            RemoveUnsettedBuilding();

            if (hit.transform.tag == this.tag && !_unitManager.SelectedUnits.Any() &&
                !_buildingManager.SelectedBuildings.Any())
            {
                ManageEmptyRightClick();
            }
        }

        private void ManageEmptyRightClick()
        {
            if (_buildingPanelUi.activeSelf)
            {
                _buildingPanelUi.SetActive(false);
            }
            else
            {
                _buildingPanelUi.SetActive(true);
            }
        }
    }
}