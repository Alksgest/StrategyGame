using UnityEngine;

using StrategyGame.Assets.Scripts.Unit;
using StrategyGame.Assets.Scripts.Util;
using System;

namespace StrategyGame.Assets.Scripts.Building
{
    public class BarrackClickHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _barrackUI;
        
        private void Awake()
        {
            var gch = FindObjectOfType<GlobalClickHandler>();
            gch.GameObjectLeftClick += OnLeftClick;
        }

        private void OnLeftClick(RaycastHit hit)
        {
            if (hit.transform.tag == this.tag || hit.transform.parent.tag == this.tag)
            {
                MakeVisibleUI();
            }
        }

        private void MakeVisibleUI()
        {
            _barrackUI.SetActive(!_barrackUI.activeSelf);
        }
    }
}