using UnityEngine;

using System.Collections;
using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.WorldState;
using UnityEngine.AI;
using StrategyGame.Assets.Scripts.Unit;
using System.Linq;

namespace StrategyGame.Assets.Scripts.UI
{
    public class BuildingsPanelManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _barrackPrefab;
        [SerializeField]
        private GameObject _minePrefab;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _buildingPanelUI;

        public bool IsBuildSelected { get; private set; } = false;

        public GameObject ObjectToCreate { get; private set; }

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private GameManager _gameManager;
        private BuildingManager _buildingManager;
        private UnitManager _unitManager;
        private NavMeshSurface _surface;

        [SerializeField]
        private WorkerController _worker;


        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.LeftMouseButtonUp += OnLeftClick;
            gch.RightMouseButtonUp += OnRightClick;

            _gameManager = FindObjectOfType<GameManager>();
            _buildingManager = FindObjectOfType<BuildingManager>();
            _unitManager = FindObjectOfType<UnitManager>();
            _surface = FindObjectOfType<NavMeshSurface>();

            _camera = Camera.main;
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
                var castPoint = Camera.main.ScreenPointToRay(mouse);

                if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
                {
                    ObjectToCreate.transform.position = new Vector3(hit.point.x, ObjectToCreate.transform.position.y, hit.point.z);
                }
            }
        }

        private void RotateBuilding()
        {
            ObjectToCreate.transform.Rotate(new Vector3(0, ObjectToCreate.transform.rotation.y + 30, 0));
        }

        private void CreateBuilding(GameObject prefab, float y)
        {
            var x = Input.GetAxis("Mouse X");
            var z = Input.GetAxis("Mouse Y");

            ObjectToCreate = Instantiate(prefab, new Vector3(x, 5, z), new Quaternion(), _buildingManager.transform);
            ObjectToCreate.layer = 8;

            _worker.IsBuilding = true;

            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void CreateBaracks()
        {
            CreateBuilding(_barrackPrefab, 5);
        }

        public void CreateMine()
        {
            CreateBuilding(_minePrefab, 4);
        }

        public void SetBuildingOnPlace(BuildingBase building)
        {
            building.Instantiate();
            ObjectToCreate = null;
            IsBuildSelected = false;
            _buildingManager.AddBuilding(building);
            _worker.IsBuilding = false;
        }

        public void RemoveUnsettedBuilding()
        {
            if (ObjectToCreate != null)
            {
                Destroy(ObjectToCreate.gameObject);
                ObjectToCreate = null;
                IsBuildSelected = false;
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
                    building != null &&
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
            RemoveUnsettedBuilding();

            if (hit.transform.tag == this.tag && !_unitManager.SelectedUnits.Any() && !_buildingManager.SelectedBuildings.Any())
            {
                ManageEmptyRightClick();
            }
        }

        private void ManageEmptyRightClick()
        {
            if (_buildingPanelUI.activeSelf)
            {
                _buildingPanelUI.SetActive(false);
            }
            else
            {
                _buildingPanelUI.SetActive(true);
            }
        }
    }
}