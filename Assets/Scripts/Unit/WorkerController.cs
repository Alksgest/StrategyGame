using System;
using Assets.Scripts.Building.Interfaces;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.UI;
using Assets.Scripts.Unit.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class WorkerController : UnitBase, IWorkable // TODO: add new interface for command such as move, attack etc.
    {
        public GameObject ObjectAttachedTo { get; set; }

        [SerializeField]
        private BuildingsPanelManager _buildingsPanelManager = null;

        [SerializeField]
        private Text _hpText = null;

        public bool IsBuilding { get; set; }
        public GameObject GameObject => this.gameObject;

        private void Start()
        {
            UnitId = Guid.NewGuid().ToString();

            Animator = GetComponent<Animator>();

            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = DefaultMaterial;
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

            if (AttackTarget != null)
            {
                Debug.Log("Attack");
                Attack();
            }

            // if (this.tag == "AttachedUnit")
            // {
            //     if (_animator != null && !_animator.GetBool("IsMining")) // TOOD: rewrite this piece
            //     {
            //         _animator.SetBool("IsMining", true);
            //     }
            // }
        }

        private void UpdateUi()
        {
            _hpText.text = $"{CurrentStats.Health}";
        }

        public override void Instantiate(UnitStats stats)
        {
            base.Instantiate(stats);
            CurrentStats = stats;
            PreviousStats = UnitStats.MakeCopy(stats);
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

        public override void Move(Vector3 point)
        {
            base.Move(point);

            // if (_animator != null)
            // {
            //     _animator.SetBool("IsRuning", true);
            // }

            if (this.tag == "AttachedUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    var workplace = ObjectAttachedTo.GetComponent<IWorkplace>();
                    workplace.DetachUnit(this);
                }

                // if (_animator != null)
                // {
                //     _animator.SetBool("IsMining", false);
                // }
            }
        }

        public void SetTag(string tag)
        {
            this.tag = tag;
        }
    }
}
