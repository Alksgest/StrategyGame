using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands;
using Assets.Scripts.Commands.Interfaces;
using Assets.Scripts.Models.Animation;
using Assets.Scripts.Models.Player;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class WorkerController : UnitBase, IWorkable // , ICommandExecutor<WorkerController>
    {
        [SerializeField] private Text _hpText = null;
        [SerializeField] private BuildingsPanelManager _buildingsPanelManager = null;

        [SerializeField] private List<WorkerTool> _tools = new List<WorkerTool>();

        private IWorkplace _workplace;

        protected Queue<IRejectableCommand<WorkerController>> WorkerRejectableCommandsQueue =
            new Queue<IRejectableCommand<WorkerController>>();

        public bool IsBuilding { get; set; }

        private void Start()
        {
            UnitId = Guid.NewGuid().ToString();

            Animator = GetComponent<Animator>();

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

        //protected override void ExecuteLastRejectableCommand()
        //{
        //    if (BaseRejectableCommandsQueue.Any())
        //    {
        //        base.ExecuteLastRejectableCommand();
        //    }
        //    else if(WorkerRejectableCommandsQueue.Any())
        //    {
        //        var command = WorkerRejectableCommandsQueue.Peek() as ICommand<WorkerController>;
        //        command?.Execute(this);
        //    }
        //}

        //protected override void RejectLastCommand()
        //{
        //    if (BaseRejectableCommandsQueue.Any())
        //    {
        //        base.RejectLastCommand();

        //    }
        //    else if (WorkerRejectableCommandsQueue.Any() && !BaseRejectableCommandsQueue.Any())
        //    {
        //        var command = WorkerRejectableCommandsQueue.Dequeue();
        //        Debug.Log($"{nameof(ICommand<WorkerController>)} was dequeue from worker queue");
        //        command.Reject(this);
        //    }
        //}

        //public void Execute(ICommand<WorkerController> command)
        //{
        //    if (command is IRejectableCommand<WorkerController> r)
        //    {
        //        RejectLastCommand();
        //        Debug.Log($"{nameof(ICommand<WorkerController>)} was enqueue to worker queue");
        //        WorkerRejectableCommandsQueue.Enqueue(r);
        //    }

        //    command.Execute(this);
        //}

        public override void TakeDamage(float value)
        {
            base.TakeDamage(value);
            UpdateUi();
        }

        private void UpdateUi()
        {
            _hpText.text = $"{CurrentStats.Health}";
        }

        public override bool Select()
        {
            if (Selected) return true;

            _buildingsPanelManager.gameObject.SetActive(true);
            return base.Select();
        }

        public override bool Deselect()
        {
            if (!Selected) return true;

            _buildingsPanelManager.gameObject.SetActive(false);
            return base.Deselect();
        }

        public override void HideUi()
        {
            UnitUi.SetActive(false);
            _buildingsPanelManager.gameObject.SetActive(false);
        }

        public override bool Delete()
        {
            Destroy(gameObject);
            Destroy(_buildingsPanelManager.gameObject);

            return true;
        }

        public bool AttachToWork(IWorkplace workplace)
        {
            if (_workplace == null)
            {
                workplace.AttacheUnit(this);
                _workplace = workplace;
                return false;
            }

            var position = workplace.GetAttachedUnitPosition(this);
            if (position == null) return true;

            var point = position.Value;

            var x = transform.position.x;
            var z = transform.position.z;

            if (Math.Abs(x - point.x) < 0.3 && Math.Abs(z - point.z) < 0.3)
            {
                if (Animator != null)
                {
                    Animator.SetBool(AnimationKind.Walking, false);
                    //InvokeRepeating(nameof(Rotate), .01f, 1.0f);
                    //CancelInvoke(nameof(Rotate));
                    Animator.SetBool(AnimationMapper.BuildingToAnimation[workplace.WorkKind], true);
                }

                var tool = _tools.Single(el => el.WorkplaceName == workplace.WorkKind).Tool;
                tool.SetActive(true);
            }

            return false;
        }

        public void DetachFromWork(IWorkplace workplace)
        {
            workplace.DetachUnit(this);
            _workplace = null;

            if (Animator != null)
            {
                Animator.SetBool(AnimationMapper.BuildingToAnimation[workplace.WorkKind], false);
                var tool = _tools.Single(el => el.WorkplaceName == workplace.WorkKind).Tool;
                tool.SetActive(false);
            }
        }
    }
}
