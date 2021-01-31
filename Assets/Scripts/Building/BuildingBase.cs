using UnityEngine;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BuildingBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _isInstantiated = false;

        public void Instantiate()
        {
            if (!_isInstantiated)
            {
                _isInstantiated = true;
            }
            else
            {
                Debug.Log("Smth went wrong. Trying to instantiate already instantiated object");
            }
        }
    }
}