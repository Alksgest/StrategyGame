using UnityEngine;

using System.Collections;
using StrategyGame.Assets.Scripts.Building;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.UI
{
    public class BuildingsPanelManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _barrackPrefab;
        [SerializeField]
        private GameObject _minePrefab;

        [SerializeField]
        private Transform _buildingsParent;


        [SerializeField]
        private Camera _camera;

        public bool IsBuildSelected { get; private set; } = false;

        public GameObject ObjectToCreate { get; private set; }

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
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

        public void CreateBaracks()
        {
            var x = Input.GetAxis("Mouse X");
            var z = Input.GetAxis("Mouse Y");

            ObjectToCreate = Instantiate(_barrackPrefab, new Vector3(x, 5, z), new Quaternion(), _buildingsParent);
            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void CreateMine()
        {
            var x = Input.GetAxis("Mouse X");
            var z = Input.GetAxis("Mouse Y");

            ObjectToCreate = Instantiate(_minePrefab, new Vector3(x, 4, z), new Quaternion(), _buildingsParent);
            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void SetBuildingOnPlace(BuildingBase building)
        {
            building.Instantiate();
            ObjectToCreate = null;
            IsBuildSelected = false;
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

                if (building != null && building.CanBePlaced)
                {
                    SetBuildingOnPlace(building);
                }
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            RemoveUnsettedBuilding();
        }
    }
}