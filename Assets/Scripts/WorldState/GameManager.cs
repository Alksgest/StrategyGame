using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using StrategyGame.Assets.Scripts.WorldState.Models;

namespace StrategyGame.Assets.Scripts.WorldState
{
    public class GameManager : MonoBehaviour
    {
        private Dictionary<string, long> BuildingCost = new Dictionary<string, long>
        {
            {"Mine", 20},
            {"Barrack", 100}
        };

        [SerializeField]
        private Text _ironCountText;
        public List<PlayerState> States { get; set; }

        private void Start()
        {
            States = new List<PlayerState>();

            States.Add(new PlayerState
            {
                Iron = 20,
                PlayerIdentifier = "mainPlayer"
            });

            SetValueToIron(20);
        }

        public bool CanPlaceBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);

            return st.Iron >= BuildingCost[buildingTag];
        }

        public void BuyBuilding(string playerIdentifier, string buildingTag)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            var cost = BuildingCost[buildingTag];

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
