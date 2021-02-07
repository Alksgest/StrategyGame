using System;

using UnityEngine;

namespace StrategyGame.Assets.Scripts.Util
{
    public class GlobalClickHandler : MonoBehaviour
    {
        public event Action<RaycastHit> GameObjectLeftClick;
        public event Action<RaycastHit> GameObjectRightClick;
        public event Action<RaycastHit> GameObjectLeftClickClickAndHold;

        private void Update()
        {
            HandleLeftMouseClick();
            HandleRightMouseClick();
            HandleLeftMouseKeyHold();
        }

        private void HandleLeftMouseKeyHold()
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        GameObjectLeftClickClickAndHold?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleRightMouseClick()
        {
            if (Input.GetMouseButtonUp((int)MouseButton.RightMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        GameObjectRightClick?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleLeftMouseClick()
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
        }
    }
}