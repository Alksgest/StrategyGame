using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Building.Interfaces;
using Assets.Scripts.Unit;
using Assets.Scripts.Unit.Interfaces;
using Assets.Scripts.WorldState;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Building
{
    public class MineController : BuildingBase, IWorkplace
    {
        private UnitManager _unitManager;
        private GameManager _gameManager;

        private Workpalce[] workplaces;

        [SerializeField]
        private Text[] _edgesText;

        [SerializeField]
        private GameObject[] _unitPlaces;

        private void Awake()
        {
            _unitManager = FindObjectOfType<UnitManager>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        public override void Instantiate()
        {
            base.Instantiate();

            InitWorkplaces();
            InvokeRepeating("AddIron", .01f, 1.0f);
        }

        private void InitWorkplaces()
        {
            workplaces = new Workpalce[_unitPlaces.Length];

            for (int i = 0; i < _unitPlaces.Length; ++i)
            {
                var pos = _unitPlaces[i].transform.position;
                workplaces[i] = new Workpalce
                {
                    Position = pos
                };
            }
        }

        private void AddIron()
        {
            foreach (var wp in workplaces)
            {
                if (wp.IsBusy && wp.IsUnitOnPlace)
                {
                    _gameManager.AddIron(Owner, 5);
                }
            }
        }

        public override void LeftClick(object obj)
        {
            if (IsInstantiated)
            {
                base.LeftClick(obj);
            }
        }

        public override void RightClick(object obj)
        {
            if (IsInstantiated)
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

                selected[i].Move(position);

                AttacheUnit(selected[i], freeWorkplaces[i]);
            }
        }

        public void AttacheUnit(IWorkable unit, Workpalce workplace)
        {
            workplace.AttachedUnit = unit;
            workplace.IsBusy = true;

            unit.SetTag("AttachedUnit");
            unit.ObjectAttachedTo = this.gameObject;

            var position = Array.IndexOf(workplaces, workplace);
            _edgesText[position].text = workplace.BusyText;
        }

        public void DetachUnit(IWorkable unit)
        {
            var workplace = workplaces.FirstOrDefault(el => el.AttachedUnit == unit);
            if (workplace != null)
            {
                DetachUnit(workplace);
            }
        }

        private void DetachUnit(Workpalce workplace)
        {
            if (workplace?.AttachedUnit != null)
            {
                workplace.AttachedUnit.SetTag("Worker");
                workplace.AttachedUnit.ObjectAttachedTo = null;
                workplace.AttachedUnit = null;

                workplace.IsBusy = false;

                var position = Array.IndexOf(workplaces, workplace);
                _edgesText[position].text = workplace.BusyText;
            }
        }

        private void OnDestroy()
        {
            foreach (var workplace in workplaces)
            {
                DetachUnit(workplace);
            }
        }
    }
}