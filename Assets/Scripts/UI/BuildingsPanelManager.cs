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

        public bool CanPlaceBuilding { get; private set; } = false;

        public GameObject ObjectToCreate { get; private set; }

        public void CreateBaracks()
        {
            ObjectToCreate = Instantiate(_barrackPrefab, new Vector3(0, 5, 0), new Quaternion(), _buildingsParent);
            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void CreateMine()
        {
            ObjectToCreate = Instantiate(_minePrefab, new Vector3(0, 4, 0), new Quaternion(), _buildingsParent);
            StartCoroutine(WaitForPlaceBuilding(0.5f));
        }

        public void SetBuildingOnPlace()
        {
            ObjectToCreate = null;
            CanPlaceBuilding = false;
        }

        private IEnumerator WaitForPlaceBuilding(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            CanPlaceBuilding = true;
        }
    }
}