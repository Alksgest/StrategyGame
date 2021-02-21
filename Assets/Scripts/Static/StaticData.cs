using System.Collections.Generic;
using Assets.Scripts.Models.Building;
using Assets.Scripts.Models.Common;
using Assets.Scripts.Models.Unit;
using UnityEngine;

namespace Assets.Scripts.Static
{
    public static class StaticData
    {
        private const string BuildingsPrefabPath = "Prefabs/Buildings";
        private const string UnitsPrefabPath = "Prefabs/Units";

        private static List<BuildingTemplate> _cachedBuildingsTemplates;
        private static List<UnitTemplate> _cachedUnitsTemplates;

        public static List<UnitTemplate> GetUnitTemplates()
        {
            return _cachedUnitsTemplates ?? (_cachedUnitsTemplates = new List<UnitTemplate>
            {
                new UnitTemplate
                {
                    Cost = new ObjectCost
                    {
                        Food = 10
                    },
                    UnitName = UnitsNames.Worker,
                    Prefab = Resources.Load<GameObject>($"{UnitsPrefabPath}/{UnitsNames.Worker}"),
                    UnitStats = new UnitStats
                    {
                        Armor = 0f,
                        Attack = 1f,
                        Defence = 1f,
                        Health = 5f,
                        Speed = 15f,
                        AttackRange = 2f
                    }
                }
            });
        }

        public static List<BuildingTemplate> GetBuildingTemplates()
        {
            return _cachedBuildingsTemplates ?? (_cachedBuildingsTemplates = new List<BuildingTemplate>
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
            });
        }
    }
}