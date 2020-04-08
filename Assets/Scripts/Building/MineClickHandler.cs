using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.Unit;

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
            _mineUI.SetActive(false);
            if (hit.transform.tag == "MineEdge" && hit.transform.parent.gameObject == this.gameObject)
            {
                _mineUI.SetActive(!_mineUI.activeSelf);
            }
        }

        private void OnMineRightClick(RaycastHit hit)
        {
            if (hit.transform.tag == "MineEdge" && hit.transform.parent.gameObject == this.gameObject)
            {
                // var edge = _edges.FirstOrDefault((e) => e.gameObject == hit.transform.gameObject);
                SendUnitsToEdge();
            }
        }

        private void SendUnitsToEdge()
        {
            var edges = _edges.Where(e => !e.IsBusy).ToList();
            var selected = _unitManager.SelectedUnits;

            if (edges.Count < selected.Count)
            {
                selected = new List<UnitController>(_unitManager.SelectedUnits.GetRange(0, edges.Count));
            }

            for (int i = 0; i < selected.Count; ++i)
            {
                selected[i].AskToMove(edges[i].GetUnitPosition());

                edges[i].AttacheUnit(selected[i]);
            }

        }
    }
}