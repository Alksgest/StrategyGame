using System;
using UnityEngine;

namespace StrategyGame.Assets.Scripts.Building
{
    public class GatesController : BuildingBase
    {
        [SerializeField]
        private Animator _animator;

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
        }

        private void OpenGates()
        {
            IsOpened = true;
            _animator.SetBool("OpenGates", true);
        }
    }
}
