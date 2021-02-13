using StrategyGame.Assets.Scripts.Models.Common;
using UnityEngine;

namespace StrategyGame.Assets.Scripts.Models.Unit
{
    public class UnitTemplate
    {
        public string UnitName { get; set; }
        public ObjectCost Cost { get; set; }
        public GameObject Prefab { get; internal set; }
    }
}