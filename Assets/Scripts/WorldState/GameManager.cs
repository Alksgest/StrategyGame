using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using StrategyGame.Assets.Scripts.WorldState.Models;

namespace StrategyGame.Assets.Scripts.WorldState
{
    public class GameManager : MonoBehaviour
    {
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

        public void AddIron(string playerIdentifier, long count)
        {
            var st = States.Find(state => state.PlayerIdentifier == playerIdentifier);
            st.Iron += count;

            SetValueToIron(st.Iron);
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
