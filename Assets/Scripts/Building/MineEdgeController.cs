using UnityEngine;
using UnityEngine.UI;

using StrategyGame.Assets.Scripts.Unit;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineEdgeController : MonoBehaviour
    {
        public bool IsBusy { get; private set; } = false;

        public UnitController AttachedUnit { get; private set; }

        [SerializeField]
        private Text _edgeText;

        [SerializeField]
        private GameObject _unitPlace;

        public void AttacheUnit(UnitController unit)
        {
            AttachedUnit = unit;

            unit.tag = "AttachedToMineUnit";
            unit.ObjectAttachedTo = this.gameObject;

            SetBusy();

            Debug.Log("Attached");
        }

        public void DeatachUnit()
        {
            if (AttachedUnit != null)
            {
                AttachedUnit.tag = "Unit";
                AttachedUnit.ObjectAttachedTo = null;
                AttachedUnit = null;
                SetBusy();

                Debug.Log("Deattached");
            }
        }

        public Vector3 GetUnitPosition()
        {
            var vec = _unitPlace.transform.position;
            vec.y = 0;

            return vec;
        }

        private void SetBusy()
        {
            IsBusy = !IsBusy;

            _edgeText.text = IsBusy ? "busy" : "free";
        }
    }
}