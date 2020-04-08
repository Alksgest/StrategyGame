using UnityEngine;
using UnityEngine.UI;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineEdgeController : MonoBehaviour
    {
        public bool IsBusy { get; private set; } = false;

        [SerializeField]
        private Text _edgeText;

        [SerializeField]
        private GameObject _unitPlace;

        public void SetBusy()
        {
            IsBusy = !IsBusy;

            _edgeText.text = IsBusy ? "busy" : "free";
        }

        public Vector3 GetUnitPosition()
        {
            var vec = _unitPlace.transform.position;
            vec.y = 0;

            return vec;
        }
    }
}