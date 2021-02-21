using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class Workplace
    {
        public bool IsBusy { get; set; } = false;
        public IWorkable AttachedUnit { get; set; }
        public string BusyText => IsBusy ? "busy" : "free";
        public Vector3 Position { get; set; }

        public bool IsUnitOnPlace
        {
            get
            {
                var unitGameObject = (AttachedUnit as UnitBase)?.gameObject;

                if (unitGameObject == null)
                {
                    return false;
                }

                var difVec = unitGameObject.transform.position - Position;

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