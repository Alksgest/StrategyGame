using UnityEngine;
using UnityEngine.EventSystems;

using StrategyGame.Assets.Scripts.Util;
using static UnityEngine.EventSystems.PointerEventData;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitClickHandler : MonoBehaviour
    {
        private UnitController _unitController;

        private void Awake()
        {
            _unitController = GetComponent<UnitController>();

            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                _unitController.Select();
            }
        }
    }
}