using Assets.Scripts.Models.Common;
using UnityEngine;

namespace Assets.Scripts.Models.Unit
{
    public class UnitTemplate
    {
        public string UnitName { get; set; }
        public ObjectCost Cost { get; set; }
        public GameObject Prefab { get; internal set; }
        public UnitStats UnitStats { get; set; }
    }
}