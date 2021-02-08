using UnityEngine;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.WorldState;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineController : BuildingBase
    {
        private UnitManager _unitManager;

        [SerializeField]
        private GameObject _mineUI;

        [SerializeField]
        private MineEdgeController[] _edges;

        [SerializeField]
        private GameManager _gameManager;

        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();

            gch.RightMouseButtonUp += OnMineRightClick;
            gch.LeftMouseButtonUp += OnMineLeftClick;

            _unitManager = FindObjectOfType<UnitManager>();
            _gameManager = FindObjectOfType<GameManager>();

            InvokeRepeating("AddIron", .01f, 1.0f);
        }


        private void AddIron()
        {
            var bisyEdges = _edges.Where(e => e.IsBusy).Count();
            if (bisyEdges > 0)
            {
                for (int i = 0; i < bisyEdges; ++i)
                {
                    _gameManager.AddIron(Owner, 5);
                }
            }
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
            var selected = _unitManager.SelectedWorkers;

            if (edges.Count < selected.Count)
            {
                selected = new List<WokerController>(_unitManager.SelectedWorkers.GetRange(0, edges.Count));
            }

            for (int i = 0; i < selected.Count; ++i)
            {
                var position = edges[i].GetUnitPosition();

                selected[i].AskToMove(position);
                edges[i].AttacheUnit(selected[i]);
            }
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();

            foreach (var edge in _edges)
            {
                edge.DeatachUnit();
            }

            if (gch != null)
            {
                gch.RightMouseButtonUp -= OnMineRightClick;
                gch.LeftMouseButtonUp -= OnMineLeftClick;
            }
        }
    }
}