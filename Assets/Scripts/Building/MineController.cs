using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.Util;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.WorldState;
using System;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineController : BuildingBase
    {
        private UnitManager _unitManager;
        private MineEdge[] _edges;

        [SerializeField]
        private Text[] _edgesText;

        [SerializeField]
        private GameObject[] _unitPlaces;

        [SerializeField]
        private GameManager _gameManager;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _gameManager = FindObjectOfType<GameManager>();

            _edges = new MineEdge[4];

            for (int i = 0; i < 4; ++i)
            {
                _edges[i] = new MineEdge
                {
                    UnitTransform = _unitPlaces[i].transform
                };
            }

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

        public override void LeftClick(object obj)
        {
            if (_isInstantiated)
            {
                base.LeftClick(obj);
            }
        }

        public override void RightClick(object obj)
        {
            if (_isInstantiated)
            {
                SendUnitsToEdge();
                base.RightClick(obj);
            }
        }


        private void SendUnitsToEdge()
        {
            var edges = _edges.Where(e => !e.IsBusy).ToList();
            var selected = _unitManager.SelectedWorkers;

            if (edges.Count < selected.Count)
            {
                selected = new List<WorkerController>(_unitManager.SelectedWorkers.GetRange(0, edges.Count));
            }

            for (int i = 0; i < selected.Count; ++i)
            {
                var position = edges[i].UnitTransform.position;

                selected[i].AskToMove(position);

                AttacheUnit(selected[i], edges[i]);
            }
        }

        public void AttacheUnit(WorkerController unit, MineEdge edge)
        {
            edge.AttachedUnit = unit;
            edge.IsBusy = true;

            unit.tag = "AttachedToMineUnit";
            unit.ObjectAttachedTo = this.gameObject;

            var position = Array.IndexOf(_edges, edge);
            _edgesText[position].text = edge.EdgeText;
        }

        public void DeatachUnit(UnitBase unit)
        {
            var edge = _edges.FirstOrDefault(el => el.AttachedUnit == unit);
            if (edge != null)
            {
                DeatachUnit(edge);
            }
        }

        private void DeatachUnit(MineEdge edge)
        {
            if (edge?.AttachedUnit != null)
            {
                edge.AttachedUnit.tag = "Unit";
                edge.AttachedUnit.ObjectAttachedTo = null;
                edge.AttachedUnit = null;

                edge.IsBusy = false;

                var position = Array.IndexOf(_edges, edge);
                _edgesText[position].text = edge.EdgeText;
            }
        }

        private void OnDestroy()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();

            foreach (var edge in _edges)
            {
                DeatachUnit(edge);
            }
        }
    }
}