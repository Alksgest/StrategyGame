using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Unit;
using Assets.Scripts.WorldState;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class FarmController : BuildingBase, IWorkplace
    {
        private UnitManager _unitManager;
        private GameManager _gameManager;

        [SerializeField] private Transform[] _unitPlaces = null;

        private Workplace[] _workplaces = null;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        public override void Instantiate()
        {
            base.Instantiate();

            InitWorkplaces();

            InvokeRepeating(nameof(AddCrop), .01f, 1.0f);
        }

        private void InitWorkplaces()
        {
            _workplaces = new Workplace[_unitPlaces.Length];

            for (var i = 0; i < _unitPlaces.Length; ++i)
            {
                var pos = _unitPlaces[i].transform.position;
                _workplaces[i] = new Workplace
                {
                    Position = pos
                };
            }
        }

        private void AddCrop()
        {
            foreach (var wp in _workplaces)
            {
                if (wp.IsBusy && wp.IsUnitOnPlace)
                {
                    _gameManager.AddFood(Owner, 5);
                }
            }
        }

        public void AttacheUnit(IWorkable unit)
        {
            var freeWorkplaces = _workplaces.Where(e => !e.IsBusy).ToList();
            //var selected = _unitManager.SelectedWorkers;

            if (freeWorkplaces.Count == 0) return;

            var workplace = freeWorkplaces.First();

            //    var position = freeWorkplaces[i].Position;
            //    selected[i].Move(position); // TODO: remove this line and add command
            AttacheUnit(unit, workplace);
        }

        private void AttacheUnit(IWorkable unit, Workplace workplace)
        {
            workplace.AttachedUnit = unit;
            workplace.IsBusy = true;

            //unit.SetTag("AttachedUnit");
            //unit.ObjectAttachedTo = this.gameObject;

            //var position = Array.IndexOf(_workplaces, workplace);
            //_edgesText[position].text = workplace.BusyText;
        }

        public void DetachUnit(IWorkable unit)
        {
            var workplace = _workplaces.FirstOrDefault(el => el.AttachedUnit == unit);
            if (workplace != null)
            {
                DetachUnit(workplace);
            }
        }

        public Vector3? GetFreePosition()
        {
            var freeWorkplaces = _workplaces.Where(e => !e.IsBusy).ToList();

            var workplace = freeWorkplaces.FirstOrDefault();

            return workplace?.Position;
        }

        private void DetachUnit(Workplace workplace)
        {
            if (workplace?.AttachedUnit != null)
            {
                //workplace.AttachedUnit.SetTag("Worker");
                //workplace.AttachedUnit.ObjectAttachedTo = null;
                workplace.AttachedUnit = null;

                workplace.IsBusy = false;

                // var position = Array.IndexOf(workplaces, workplace);
                // _edgesText[position].text = workplace.EdgeText;
            }
        }

        //public override void RightClick(object obj)
        //{
        //    if (!IsInstantiated) return;

        //    SendUnitsToWorkplace();
        //    base.RightClick(obj);
        //}

    }
}