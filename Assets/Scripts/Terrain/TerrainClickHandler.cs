using UnityEngine;

using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.Terrain
{
    public class TerrainClickHandler : MonoBehaviour
    {
        private UnitManager _unitManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
            gch.GameObjectRightClick += OnRightClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            if (hit.transform.tag == this.tag)
            {
                _unitManager.DeselectAll();
            }
        }

        private void OnRightClick(RaycastHit hit)
        {
            if (hit.transform.tag == this.tag)
            {
                _unitManager.MoveUnitsToPoint(hit.point);
            }

        }


    }
}