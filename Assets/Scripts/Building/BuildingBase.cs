using StrategyGame.Assets.Scripts.UI;
using UnityEngine;

namespace StrategyGame.Assets.Scripts.Building
{
    public abstract class BuildingBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _isInstantiated = false;

        [SerializeField]
        protected GameObject _UI;

        public string Owner { get; protected set; } = "mainPlayer";

        public bool CanBePlaced { get; protected set; } = true;

        public bool Selected { get; protected set; } = false;

        private BuildingsPanelManager _buildingsPanelManager;

        public void Instantiate()
        {
            if (!_isInstantiated)
            {
                _isInstantiated = true;
            }
            else
            {
                Debug.LogError("Smth went wrong. Trying to instantiate already instantiated object");
            }
        }

        public virtual void LeftClick(object obj)
        {
            if (_buildingsPanelManager == null)
            {
                _buildingsPanelManager = FindObjectOfType<BuildingsPanelManager>();
            }

            if (!_buildingsPanelManager.IsBuildSelected)
            {
                SetUIActive();
                Selected = true;
            }
        }

        public void Deselect()
        {
            if (_buildingsPanelManager == null)
            {
                _buildingsPanelManager = FindObjectOfType<BuildingsPanelManager>();
            }
            if (!_buildingsPanelManager.IsBuildSelected)
            {
                SetUIInactive();
                Selected = false;
            }
        }

        public virtual void RightClick(object obj)
        {

        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetUIActive()
        {
            _UI.SetActive(true);
        }

        public void SetUIInactive()
        {
            _UI.SetActive(false);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isInstantiated)
            {
                CanBePlaced = false;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (_isInstantiated)
            {
                CanBePlaced = true;
            }
        }
    }
}