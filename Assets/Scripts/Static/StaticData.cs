using UnityEngine;

using System.Collections.Generic;
using StrategyGame.Assets.Scripts.Models.Building;

namespace StrategyGame.Assets.Scripts.Static
{
    public static class StaticData
    {
        private static string BuildingsPrefabPath = "Prefabs/Buildings";
        private static List<BuildingTemplate> _cachedTemplates;

        public static List<BuildingTemplate> GetBuildingTemplates()
        {
            if (_cachedTemplates == null)
            {
                _cachedTemplates = new List<BuildingTemplate>
                {
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Mine,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 20
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Mine}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 4
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Barracks,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 60
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Barracks}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Farm,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 20
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Farm}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.ShootingRange,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 100
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.ShootingRange}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 4
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.CastleWall,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 200
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.CastleWall}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 4
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Gates,
                        BuildingCost = new BuildingCost
                        {
                            Iron = 100
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Gates}"),
                        BuildingAccessLevel = BuildingAccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 4
                        }
                    },
                };
            }
            return _cachedTemplates;
        }
    }
}