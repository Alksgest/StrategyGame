using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.WorldState.Models;
using StrategyGame.Assets.Scripts.Models.Building;
using StrategyGame.Assets.Scripts.Static;
using StrategyGame.Assets.Scripts.Models.Unit;

namespace StrategyGame.Assets.Scripts.WorldState
{
    public class GameManager : MonoBehaviour
    {
        private List<BuildingTemplate> _buildings;
        private List<UnitTemplate> _units;

        [SerializeField]
        private Text _ironCountText;

        [SerializeField]
        private Text _foodCountText;

        public List<PlayerState> States { get; set; }

        private void Start()
        {
            States = new List<PlayerState>();

            States.Add(new PlayerState
            {
                Iron = 20,
                Food = 20,
                PlayerIdentifier = "mainPlayer"
            });

            _buildings = StaticData.GetBuildingTemplates();
            _units = StaticData.GetUnitTemplates();

            AddIron("mainPlayer", 20);
            AddFood("mainPlayer", 20);
        }

        public bool CanBuyUnit(string playerIdentifier, string unitTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);

            var u = _units.Single(el => el.UnitName == unitTag);

            return st.Food >= u.Cost.Food;
        }

        public bool CanPlaceBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);

            var b = _buildings.Single(el => el.BuildingName == buildingTag);

            return st.Iron >= b.Cost.Iron;
        }

        public void BuyUnit(string playerIdentifier, string unitTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            var b = _units.Single(el => el.UnitName == unitTag);
            var cost = b.Cost.Food;

            if (st.Food < cost)
            {
                Debug.LogError($"You cannot by {unitTag} for {playerIdentifier}.");
                return;
            }

            AddFood(playerIdentifier, -cost);
        }

        public void BuyBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            var b = _buildings.Single(el => el.BuildingName == buildingTag);
            var cost = b.Cost.Iron;

            if (st.Iron < cost)
            {
                Debug.LogError($"You cannot by {buildingTag} for {playerIdentifier}.");
                return;
            }

            AddIron(playerIdentifier, -cost);
        }

        public void AddIron(string playerIdentifier, long count)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            st.Iron += count;

            if (_ironCountText != null)
            {
                _ironCountText.text = $"{st.Iron}";
            }
        }

        public void AddFood(string playerIdentifier, long count)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            st.Food += count;

            if (_foodCountText != null)
            {
                _foodCountText.text = $"{st.Food}";
            }
        }
    }
}
