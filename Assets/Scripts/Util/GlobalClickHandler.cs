using System;

using UnityEngine;

namespace StrategyGame.Assets.Scripts.Util
{
    public class GlobalClickHandler : MonoBehaviour
    {
        public event Action<RaycastHit> LeftMouseButtonUp;
        public event Action<RaycastHit> LeftMouseButtonHold;
        public event Action<RaycastHit> LeftMouseButtonDown;
        public event Action<RaycastHit> RightMouseButtonUp;

        private void Update()
        {
            HandleLeftMouseButtonUp();
            HandleLeftMouseButtonHold();
            HandleLeftMouseButtonDown();
            HandleRightMouseButtonUp();
        }
        
        private void HandleLeftMouseButtonDown()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        LeftMouseButtonDown?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleLeftMouseButtonHold()
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        LeftMouseButtonHold?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleRightMouseButtonUp()
        {
            if (Input.GetMouseButtonUp((int)MouseButton.RightMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        RightMouseButtonUp?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleLeftMouseButtonUp()
        {
            if (Input.GetMouseButtonUp((int)MouseButton.LeftMouseButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        LeftMouseButtonUp?.Invoke(hit);
                    }
                }
            }
        }
    }
}