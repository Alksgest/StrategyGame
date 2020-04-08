using UnityEngine;

using StrategyGame.Assets.Scripts.Unit;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BarrackController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _unitPrefab;
        [SerializeField]
        private GameObject _barrackUI;

        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
        }

        public void CreateNewUnit()
        {
            _barrackUI.SetActive(true);

            _unitManager.CreateUnit(_unitPrefab, this.transform.position + new Vector3(0, 0, 10));
        }
    }
}