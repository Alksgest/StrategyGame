using System.Linq;
using UnityEngine;

using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.Unit;
using System;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineClickHandler : MonoBehaviour
    {
        private UnitManager _unitManager;

        [SerializeField]
        private GameObject _mineUI;

        [SerializeField]
        private MineEdgeController[] _edges;

        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();

            gch.GameObjectRightClick += OnMineRightClick;
            gch.GameObjectLeftClick += OnMineLeftClick;

            _unitManager = FindObjectOfType<UnitManager>();
        }

        private void OnMineLeftClick(RaycastHit hit)
        {
            if (hit.transform.tag == "Mine" || hit.transform.tag == "MineEdge")
            {
                _mineUI.SetActive(!_mineUI.activeSelf);
            }
        }

        private void OnMineRightClick(RaycastHit hit)
        {
            // hit.transform.tag == "Mine" ||
            if (hit.transform.tag == "MineEdge")
            {
                // var edge = _edges.FirstOrDefault((e) => e.gameObject == hit.transform.gameObject);
                var edge = _edges.FirstOrDefault((e) => e.IsBusy == false);

                if (edge != null)
                {
                    edge.SetBusy();
                    _unitManager.MoveUnitsToPoint(edge.GetUnitPosition());
                    Debug.Log(edge.GetUnitPosition());
                }
            }
        }
    }
}