using System;
using System.Linq;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Unit;
using Assets.Scripts.WorldState;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Building
{
    public class MineController : BuildingBase, IWorkplace
    {
        private GameManager _gameManager;

        private Workplace[] _workplaces;

        [SerializeField] private Text[] _edgesText;
        [SerializeField] private GameObject[] _unitPlaces;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public override void Instantiate()
        {
            base.Instantiate();

            InitWorkplaces();
            InvokeRepeating(nameof(AddIron), .01f, 1.0f);
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

        private void AddIron()
        {
            foreach (var wp in _workplaces)
            {
                if (wp.IsBusy && wp.IsUnitOnPlace)
                {
                    _gameManager.AddIron(Owner, 5);
                }
            }
        }

        public void AttacheUnit(IWorkable unit)
        {
            var freeWorkplaces = _workplaces.Where(e => !e.IsBusy).ToList();

            if (freeWorkplaces.Count == 0) return;

            var workplace = freeWorkplaces.First();

            AttacheUnit(unit, workplace);
        }

        private void AttacheUnit(IWorkable unit, Workplace workplace)
        {
            workplace.AttachedUnit = unit;
            workplace.IsBusy = true;

            var index = Array.IndexOf(_workplaces, workplace);
            _edgesText[index].text = workplace.BusyText;
        }

        public void DetachUnit(IWorkable unit)
        {
            var workplace = _workplaces.FirstOrDefault(el => el.AttachedUnit == unit);
            if (workplace?.AttachedUnit != null)
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
            if (workplace?.AttachedUnit == null) return;

            workplace.AttachedUnit = null;

            workplace.IsBusy = false;

            var index = Array.IndexOf(_workplaces, workplace);
            _edgesText[index].text = workplace.BusyText;
        }

        private void OnDestroy()
        {
            foreach (var workplace in _workplaces)
            {
                DetachUnit(workplace);
            }
        }
    }
}