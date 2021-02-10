using System;
using UnityEngine;
using UnityEngine.AI;

namespace StrategyGame.Assets.Scripts.Building
{
    public class GatesController : BuildingBase
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private NavMeshObstacle _navMeshObstacle;

        public bool IsOpened { get; private set; } = false;


        public override void LeftClick(object obj)
        {
            base.LeftClick(obj);
        }

        public void OpenOrCloseGates()
        {
            if (IsOpened)
            {
                CloseGates();
            }
            else
            {
                OpenGates();
            }

        }

        private void CloseGates()
        {
            IsOpened = false;
            _animator.SetBool("OpenGates", false);
            _navMeshObstacle.enabled = true;
        }

        private void OpenGates()
        {
            IsOpened = true;
            _animator.SetBool("OpenGates", true);
            _navMeshObstacle.enabled = false;
        }
    }
}
