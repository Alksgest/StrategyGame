using UnityEngine;

namespace Assets.Scripts.Building
{
    public class BarrackController : BuildingBase
    {
        [SerializeField]
        private GameObject _barracksUI = null;

        private void Awake()
        {
            IsInstantiated = false;
        }

        private void OnDestroy()
        {
            IsInstantiated = false;
        }

        public override bool Select()
        {
            base.Select();
            _barracksUI.SetActive(true);

            return true;
        }

        public override bool Deselect()
        {
            base.Deselect();
            _barracksUI.SetActive(false);

            return true;
        }
    }
}