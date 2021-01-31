using UnityEngine;

using System.Collections;

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

        public bool CanPlaceBuilding { get; private set; } = false;

        public GameObject ObjectToCreate { get; private set; }

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private void FixedUpdate()
        {
            if (ObjectToCreate != null)
            {
                var mouse = Input.mousePosition;
                var castPoint = Camera.main.ScreenPointToRay(mouse);

                if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
                {
                    ObjectToCreate.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                }
            }
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

        public void SetBuildingOnPlace()
        {
            ObjectToCreate = null;
            CanPlaceBuilding = false;
        }

        public void RemoveUnsettedBuilding()
        {
            if (ObjectToCreate != null)
            {
                Destroy(ObjectToCreate.gameObject);
                ObjectToCreate = null;
                CanPlaceBuilding = false;
            }
        }

        private IEnumerator WaitForPlaceBuilding(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            CanPlaceBuilding = true;
        }
    }
}