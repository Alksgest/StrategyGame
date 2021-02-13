using UnityEngine;
using StrategyGame.Assets.Scripts.Unit.Interfaces;

namespace StrategyGame.Assets.Scripts.Building
{
    public class Workpalce
    {
        public bool IsBusy { get; set; } = false;
        public IWorkable AttachedUnit { get; set; }
        public string BusyText => IsBusy ? "busy" : "free";
        public Vector3 Position { get; set; }

        public bool IsUnitOnPlace
        {
            get
            {
                if (AttachedUnit == null)
                {
                    return false;
                }
                
                var difVec = AttachedUnit.GameObject.transform.position - Position;

                var dx = difVec.x;
                // var dy = difVec.y;
                var dz = difVec.z;

                const float maxX = 0.5f;
                // const float maxY = 0.5f;
                const float maxZ = 0.5f;

                return Mathf.Abs(dx) < maxX && Mathf.Abs(dz) < maxZ; // && Mathf.Abs(dy) < maxY 
            }
        }
    }
}