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

        public override void Select()
        {
            base.Select();
            _barracksUI.SetActive(true);
        }

        public override void Deselect()
        {
            base.Deselect();
            _barracksUI.SetActive(false);
        }
    }
}