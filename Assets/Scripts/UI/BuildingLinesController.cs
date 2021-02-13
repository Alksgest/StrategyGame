using System.Linq;
using System.Collections.Generic;
using StrategyGame.Assets.Scripts.Models.Building;
using StrategyGame.Assets.Scripts.Static;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyGame.Assets.Scripts.UI
{
    public class BuildingLinesController : MonoBehaviour
    {
        private List<BuildingTemplate> _buildings;

        [SerializeField]
        private GameObject BuildingLinePrefab;

        [SerializeField]
        private BuildingsPanelManager _buildingsPanelManager;

        private Vector3 _initialPosition = new Vector3(-130 + 260, 60 + 199, 0); //  very magic constants in addition. Very strange behaviour
        private int _yDelta = 30;

        private void Awake()
        {
            _buildings = StaticData.GetBuildingTemplates();

            CreateBuildingLines();
        }

        private void CreateBuildingLines()
        {
            foreach (var b in _buildings) //  _buildings.Take(5)
            {
                var line = GameObject.Instantiate(BuildingLinePrefab, Vector3.zero, new Quaternion(0, 0, 0, 0), this.transform);
                line.transform.position = _initialPosition;
                _initialPosition.y = _initialPosition.y - _yDelta;

                var button = line.GetComponentInChildren<Button>();
                var buttonText = button.gameObject.GetComponentInChildren<Text>();
                var costText = line.GetComponentInChildren<Text>();

                button.onClick.AddListener(() =>
                {
                    Debug.Log(b.BuildingName);
                    _buildingsPanelManager.CreateBuilding(b.Prefab, b.BuildingParams.Y);
                });

                buttonText.text = b.BuildingName;
                costText.text = $"Iron: {b.Cost.Iron}";
            }
        }
    }
}