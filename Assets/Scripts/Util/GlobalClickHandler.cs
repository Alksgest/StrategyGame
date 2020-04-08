using System;

using UnityEngine;

namespace StrategyGame.Assets.Scripts.Util
{
    public class GlobalClickHandler : MonoBehaviour
    {
        public event Action<RaycastHit> GameObjectLeftClick;
        public event Action<RaycastHit> GameObjectRightClick;
        private void Update()
        {
            if (Input.GetMouseButtonUp((int)MouseButton.LeftMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        GameObjectLeftClick?.Invoke(hit);
                    }
                }
            }

            if (Input.GetMouseButtonUp((int)MouseButton.RightMouseButton))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform != null)
                    {
                        GameObjectRightClick?.Invoke(hit);
                    }
                }
            }
        }
    }
}