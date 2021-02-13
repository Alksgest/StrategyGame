using UnityEngine;

using StrategyGame.Assets.Scripts.Building.Interfaces;
using StrategyGame.Assets.Scripts.Unit.Interfaces;
using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.WorldState;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.Assets.Scripts.Building
{
    public class FarmController : BuildingBase, IWorkplace
    {

        private UnitManager _unitManager;
        private GameManager _gameManager;


        [SerializeField]
        private Transform[] _unitPlaces;
        private Workpalce[] workplaces;


        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _gameManager = FindObjectOfType<GameManager>();

            workplaces = new Workpalce[_unitPlaces.Length];

            for (int i = 0; i < _unitPlaces.Length; ++i)
            {
                workplaces[i] = new Workpalce
                {
                    Position = _unitPlaces[i].transform.position,
                    AttachedUnit = null,
                    IsBusy = false
                };
            }

            // InvokeRepeating("AddIron", .01f, 1.0f);
        }

        public void AttacheUnit(IWorkable unit, Workpalce workplace)
        {
            workplace.AttachedUnit = unit;
            workplace.IsBusy = true;

            unit.SetTag("AttachedUnit");
            unit.ObjectAttachedTo = this.gameObject;

            var position = Array.IndexOf(workplaces, workplace);
            // _edgesText[position].text = workplace.EdgeText;
        }

        public void DeatachUnit(IWorkable unit)
        {
            var workplace = workplaces.FirstOrDefault(el => el.AttachedUnit == unit);
            if (workplace != null)
            {
                DeatachUnit(workplace);
            }
        }

        private void DeatachUnit(Workpalce workplace)
        {
            if (workplace?.AttachedUnit != null)
            {
                workplace.AttachedUnit.SetTag("Unit");
                workplace.AttachedUnit.ObjectAttachedTo = null;
                workplace.AttachedUnit = null;

                workplace.IsBusy = false;

                // var position = Array.IndexOf(workplaces, workplace);
                // _edgesText[position].text = workplace.EdgeText;
            }
        }

        public override void RightClick(object obj)
        {
            if (_isInstantiated)
            {
                SendUnitsToWorkplace();
                base.RightClick(obj);
            }
        }

        private void SendUnitsToWorkplace()
        {
            var freeWorkplaces = workplaces.Where(e => !e.IsBusy).ToList();
            var selected = _unitManager.SelectedWorkers;

            if (freeWorkplaces.Count < selected.Count)
            {
                selected = new List<WorkerController>(_unitManager.SelectedWorkers.GetRange(0, freeWorkplaces.Count));
            }

            for (int i = 0; i < selected.Count; ++i)
            {
                var position = freeWorkplaces[i].Position;

                selected[i].AskToMove(position);

                AttacheUnit(selected[i], freeWorkplaces[i]);
            }
        }
    }
}