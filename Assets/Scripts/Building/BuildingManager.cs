using UnityEngine;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BuildingManager : MonoBehaviour
    {
        private void Awake()
        {
            var buildings = FindObjectsOfType<BuildingBase>();
            foreach (var b in buildings)
            {
                b.Instantiate();
            }
        }
    }
}