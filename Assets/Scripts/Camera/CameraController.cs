using UnityEngine;

using StrategyGame.Assets.Scripts.Util;

namespace StrategyGame.Assets.Scripts.UI
{
    public partial class CameraController : MonoBehaviour
    {

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private float _scrollCoefficient = 3.0f;
        [SerializeField]
        private float _moveCoefficient = 3.0f;
        [SerializeField]
        private float _rotateCoefficient = 2.0f;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            ScrollCamera();
            MoveCamera();
            RotateCamera();
        }

        private void RotateCamera()
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouseButton) &&
                Input.GetMouseButton((int)MouseButton.RightMouseButton))
            {
                var x = Input.GetAxis("Mouse Y") * _rotateCoefficient;

                var cameraRotationX = _camera.transform.rotation.eulerAngles.x;

                if ((cameraRotationX + x >= 20 && cameraRotationX + x <= 90))
                    _camera.transform.Rotate(x, 0, 0);
            }
        }

        private void MoveCamera()
        {
            if (Input.GetMouseButton((int)MouseButton.MiddleMouseButton))
            {
                var x = Input.GetAxis("Mouse X") * _moveCoefficient;
                var y = Input.GetAxis("Mouse Y") * _moveCoefficient;

                _camera.transform.Translate(-x, -y, 0);
            }
        }

        private void ScrollCamera()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                var newY = _camera.transform.position.y - (Input.mouseScrollDelta.y * _scrollCoefficient);
                newY = Mathf.Clamp(newY, 20, 300);
                _camera.transform.position =
                new Vector3(_camera.transform.position.x,
                newY,
                _camera.transform.position.z);
            }
        }
    }
}