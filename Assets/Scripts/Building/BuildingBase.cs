using UnityEngine;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BuildingBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _isInstantiated = false;

        public string Owner { get; private set; } = "mainPlayer";

        public bool CanBePlaced { get; private set; } = true;

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

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isInstantiated)
            {
                CanBePlaced = false;
                // if (other.gameObject.tag == "Terraine")
                // {
                //     Debug.Log("in true");
                //     CanBePlaced = true;
                // }
                // else
                // {
                //     Debug.Log("in false");
                //     CanBePlaced = false;
                // }
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