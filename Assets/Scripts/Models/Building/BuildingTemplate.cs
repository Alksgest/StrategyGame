using StrategyGame.Assets.Scripts.Models.Common;
using UnityEngine;

namespace StrategyGame.Assets.Scripts.Models.Building
{

    public class BuildingTemplate
    {
        public string BuildingName { get; set; }
        public ObjectCost Cost { get; set; }
        public GameObject Prefab { get; set; }
        public AccessLevel BuildingAccessLevel { get; set; }
        public BuildingParams BuildingParams { get; set; }
    }
}