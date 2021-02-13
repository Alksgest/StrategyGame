using UnityEngine;

namespace StrategyGame.Assets.Scripts.Models.Building
{

    public class BuildingTemplate
    {
        public string BuildingName { get; set; }
        public BuildingCost BuildingCost { get; set; }
        public GameObject Prefab { get; set; }
        public BuildingAccessLevel BuildingAccessLevel { get; set; }
        public BuildingParams BuildingParams { get; set; }
    }
}