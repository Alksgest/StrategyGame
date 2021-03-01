using System;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class GlobalClickHandler : MonoBehaviour
    {
        public event Action<RaycastHit> LeftMouseButtonUp;
        public event Action<RaycastHit> LeftMouseButtonHold;
        public event Action<RaycastHit> LeftMouseButtonDown;
        public event Action<RaycastHit> RightMouseButtonUp;
        public event Action<RaycastHit> RightMouseButtonHold;
        public event Action<RaycastHit> RightMouseButtonDown;

        private void Update()
        {
            HandleLeftMouseButtonDown();
            HandleLeftMouseButtonHold();
            HandleLeftMouseButtonUp();
            HandleRightMouseButtonDown();
            HandleRightMouseButtonHold();
            HandleRightMouseButtonUp();
        }

        private void HandleRightMouseButtonDown()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouseButton))
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        RightMouseButtonDown?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleRightMouseButtonHold()
        {
            if (Input.GetMouseButton((int)MouseButton.RightMouseButton))
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null)
                    {
                        RightMouseButtonHold?.Invoke(hit);
                    }
                }
            }
        }

        private void HandleLeftMouseButtonDown()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouseButton))
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

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
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

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
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

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
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

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