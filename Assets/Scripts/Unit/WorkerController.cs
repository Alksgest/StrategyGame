using System;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Animation;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class WorkerController : UnitBase, IWorkable, ICommandExecutor<WorkerController>
    {
        [SerializeField] private Text _hpText = null;
        [SerializeField] private BuildingsPanelManager _buildingsPanelManager = null;

        private IRejectableCommand<WorkerController> _lastRejectableCommand;
        private IWorkplace _workplace;

        public bool IsBuilding { get; set; }

        private void Start()
        {
            UnitId = Guid.NewGuid().ToString();

            Animator = GetComponent<Animator>();

            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = DefaultMaterial;
            }

            InvokeRepeating(nameof(ExecuteLastRejectableCommand), .01f, 0.1f);
        }

        private void Awake()
        {
            if (NavMeshAgent == null)
            {
                NavMeshAgent = GetComponent<NavMeshAgent>();
            }

            if (_hpText != null)
            {
                UpdateUi();
            }
        }

        private void FixedUpdate()
        {
            if (!Equals(CurrentStats, PreviousStats))
            {
                PreviousStats = UnitStats.MakeCopy(CurrentStats);

                UpdateUi();
                NavMeshAgent.speed = CurrentStats.Speed;
            }
        }

        protected override void ExecuteLastRejectableCommand()
        {
            if (LastRejectableCommand != null)
            {
                base.ExecuteLastRejectableCommand();

            }
            else
            {
                (_lastRejectableCommand as ICommand<WorkerController>)?.Execute(this);
            }
        }

        protected override void RejectLastCommand()
        {
            if (LastRejectableCommand != null)
            {
                base.RejectLastCommand();

            }
            else if (_lastRejectableCommand != null)
            {
                _lastRejectableCommand.Reject(this);
                _lastRejectableCommand = null;
            }
        }

        public void Execute(ICommand<WorkerController> command)
        {
            if (command is IRejectableCommand<WorkerController> r)
            {
                RejectLastCommand();
                _lastRejectableCommand = r;
            }

            command.Execute(this);
        }

        public override void TakeDamage(float value)
        {
            base.TakeDamage(value);
            UpdateUi();
        }

        private void UpdateUi()
        {
            _hpText.text = $"{CurrentStats.Health}";
        }

        public override void Select()
        {
            if (Selected) return;

            _buildingsPanelManager.gameObject.SetActive(true);
            base.Select();
        }

        public override void Deselect()
        {
            if (!Selected) return;

            _buildingsPanelManager.gameObject.SetActive(false);
            base.Deselect();
        }

        public override void HideUi()
        {
            UnitUi.SetActive(false);
            _buildingsPanelManager.gameObject.SetActive(false);
        }

        public override void Delete()
        {
            Destroy(gameObject);
            Destroy(_buildingsPanelManager.gameObject);
        }

        public void AttachToWork(IWorkplace workplace)
        {
            if (_workplace == null)
            {
                var pos = workplace.GetFreePosition();
                if (pos == null) return;

                workplace.AttacheUnit(this);
                Move(pos.Value);

                _workplace = workplace;
                return;
            }

            if (Animator != null)
            {
                var position = workplace.GetAttachedUnitPosition(this);

                if (position == null) return;

                var point = position.Value;

                var x = transform.position.x;
                var z = transform.position.z;

                if (Math.Abs(x - point.x) < 0.3 && Math.Abs(z - point.z) < 0.3)
                {
                    Animator.SetBool(AnimationKind.Walking, false);
                    Animator.SetBool(AnimationMapper.BuildingToAnimation[workplace.WorkKind], true);
                }
            }
        }

        public void DetachFromWork(IWorkplace workplace)
        {
            workplace.DetachUnit(this);
            _workplace = null;

            if (Animator != null)
            {
                Animator.SetBool(AnimationMapper.BuildingToAnimation[workplace.WorkKind], false);
            }
        }
    }
}
