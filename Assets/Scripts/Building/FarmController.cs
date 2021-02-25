using System.Linq;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Models.Building;
using Assets.Scripts.Unit;
using Assets.Scripts.WorldState;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class FarmController : BuildingBase, IWorkplace
    {
        private GameManager _gameManager;
        private Workplace[] _workplaces = null;

        public string WorkKind => BuildingsNames.Farm;

        [SerializeField] private Transform[] _unitPlaces = null;

        private void Awake()
        {
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

            if (freeWorkplaces.Count == 0) return;

            var workplace = freeWorkplaces.First();

            AttacheUnit(unit, workplace);
        }

        private void AttacheUnit(IWorkable unit, Workplace workplace)
        {
            workplace.AttachedUnit = unit;
            workplace.IsBusy = true;
        }

        public void DetachUnit(IWorkable unit)
        {
            var workplace = _workplaces.FirstOrDefault(el => el.AttachedUnit == unit);
            if (workplace != null)
            {
                DetachUnit(workplace);
            }
        }

        private void DetachUnit(Workplace workplace)
        {
            if (workplace?.AttachedUnit != null)
            {
                workplace.AttachedUnit = null;
                workplace.IsBusy = false;
            }
        }

        public Vector3? GetFreePosition()
        {
            var freeWorkplaces = _workplaces.Where(e => !e.IsBusy).ToList();

            var workplace = freeWorkplaces.FirstOrDefault();

            return workplace?.Position;
        }

        public Vector3? GetAttachedUnitPosition(IWorkable unit)
        {
            var workplace = _workplaces.SingleOrDefault(w => w.IsBusy && w.AttachedUnit == unit);

            return workplace?.Position;
        }

    }
}