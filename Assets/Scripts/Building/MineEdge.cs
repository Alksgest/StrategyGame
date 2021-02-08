using UnityEngine;
using StrategyGame.Assets.Scripts.Unit;
using UnityEngine.UI;

namespace StrategyGame.Assets.Scripts.Building
{
    public class MineEdge
    {
        public bool IsBusy { get; set; } = false;
        public WorkerController AttachedUnit { get; set; }
        public string EdgeText => IsBusy ? "busy" : "free";
        public Transform UnitTransform { get; set; }
    }
}