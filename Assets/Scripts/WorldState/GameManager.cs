using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections.Generic;

using StrategyGame.Assets.Scripts.WorldState.Models;
using StrategyGame.Assets.Scripts.Models.Building;
using StrategyGame.Assets.Scripts.Static;

namespace StrategyGame.Assets.Scripts.WorldState
{
    public class GameManager : MonoBehaviour
    {
        private List<BuildingTemplate> _buildings;

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
                PlayerIdentifier = "mainPlayer"
            });

            _buildings = StaticData.GetBuildingTemplates();

            SetValueToIron(20);
        }

        public bool CanPlaceBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);

            var b = _buildings.Single(el => el.BuildingName == buildingTag);

            return st.Iron >= b.BuildingCost.Iron;
        }

        public void BuyBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            var b = _buildings.Single(el => el.BuildingName == buildingTag);
            var cost = b.BuildingCost.Iron;

            if (st.Iron < cost)
            {
                Debug.LogError($"You cannt by {buildingTag} for {playerIdentifier}.");
                return;
            }

            AddIron(st, -cost);
        }

        public void AddIron(string playerIdentifier, long count)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            AddIron(st, count);
        }

        public void AddFood(string playerIdentifier, long count)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            AddFood(st, count);
        }

        private void AddFood(PlayerState state, long count)
        {
            state.Food += count;

            SetValueToFood(state.Food);
        }

        private void SetValueToFood(long food)
        {
            if (_foodCountText != null)
            {
                _foodCountText.text = $"{food}";
            }
        }

        private void AddIron(PlayerState state, long count)
        {
            state.Iron += count;

            SetValueToIron(state.Iron);
        }

        private void SetValueToIron(long iron)
        {
            if (_ironCountText != null)
            {
                _ironCountText.text = $"{iron}";
            }
        }
    }
}
