using UnityEngine;

using System.Collections.Generic;
using StrategyGame.Assets.Scripts.Models.Building;
using StrategyGame.Assets.Scripts.Models.Common;
using StrategyGame.Assets.Scripts.Models.Unit;

namespace StrategyGame.Assets.Scripts.Static
{
    public static class StaticData
    {
        private static string BuildingsPrefabPath = "Prefabs/Buildings";
        private static List<BuildingTemplate> _cachedBuildingsTemplates;
        private static List<UnitTemplate> _cachedUnitsTemplates;

        public static List<UnitTemplate> GetUnitTemplates()
        {
            if (_cachedUnitsTemplates == null)
            {
                _cachedUnitsTemplates = new List<UnitTemplate>
                {

                };
            }

            return _cachedUnitsTemplates;
        }

        public static List<BuildingTemplate> GetBuildingTemplates()
        {
            if (_cachedBuildingsTemplates == null)
            {
                _cachedBuildingsTemplates = new List<BuildingTemplate>
                {
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Mine,
                        Cost = new ObjectCost
                        {
                            Iron = 20
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Mine}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 4
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Barracks,
                        Cost = new ObjectCost
                        {
                            Iron = 60
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Barracks}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Farm,
                        Cost = new ObjectCost
                        {
                            Iron = 20
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Farm}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.ShootingRange,
                        Cost = new ObjectCost
                        {
                            Iron = 100
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.ShootingRange}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.CastleWall,
                        Cost = new ObjectCost
                        {
                            Iron = 200
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.CastleWall}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                    new BuildingTemplate
                    {
                        BuildingName = BuildingsNames.Gates,
                        Cost = new ObjectCost
                        {
                            Iron = 100
                        },
                        Prefab = Resources.Load<GameObject>($"{BuildingsPrefabPath}/{BuildingsNames.Gates}"),
                        BuildingAccessLevel = AccessLevel.BaseLevel,
                        BuildingParams = new BuildingParams
                        {
                            Y = 0
                        }
                    },
                };
            }
            return _cachedBuildingsTemplates;
        }
    }
}