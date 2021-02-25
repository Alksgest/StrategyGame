using System;
using Assets.Scripts.Models.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unit
{
    public class UnitController : UnitBase
    {
        [SerializeField]
        private Text _speedText;

        private void Start()
        {
            UnitId = Guid.NewGuid().ToString();

            Animator = GetComponent<Animator>();

            PreviousStats = CurrentStats;
        }

        private void Awake()
        {
            if (_speedText != null)
            {
                _speedText.text = $"{CurrentStats.Speed}";
            }
        }

        private void FixedUpdate()
        {
            if (!Equals(CurrentStats, PreviousStats))
            {
                PreviousStats = UnitStats.MakeCopy(CurrentStats);

                _speedText.text = $"{CurrentStats.Speed}";
                NavMeshAgent.speed = CurrentStats.Speed;
            }
        }

        public override void HideUi()
        {
            UnitUi.SetActive(false);
        }

    }
}
