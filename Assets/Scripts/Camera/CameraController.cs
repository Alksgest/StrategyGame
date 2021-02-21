﻿using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        private UnityEngine.Camera _camera;

        [SerializeField]
        private float _scrollCoefficient = 500.0f;
        [SerializeField]
        private float _moveCoefficient = 300.0f;
        [SerializeField]
        private float _rotateCoefficient = 100.0f;

        private float _rotationX = 45f;
        private float _rotationY = 180f;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            ScrollCamera();
            MoveCamera();
            RotateCamera();
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                var z = _moveCoefficient * Time.deltaTime;
                _camera.transform.Translate(0, z, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                var z = _moveCoefficient * Time.deltaTime;
                _camera.transform.Translate(0, -z, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                var x = _moveCoefficient * Time.deltaTime;
                _camera.transform.Translate(-x, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                var x = _moveCoefficient * Time.deltaTime;
                _camera.transform.Translate(x, 0, 0);
            }
        }

        private void RotateCamera()
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouseButton) &&
                Input.GetMouseButton((int)MouseButton.RightMouseButton))
            {
                _rotationY += (Input.GetAxis("Mouse X") * _rotateCoefficient * Time.deltaTime);
                _rotationX += (Input.GetAxis("Mouse Y") * _rotateCoefficient * Time.deltaTime * -1);

                _rotationX = Mathf.Clamp(_rotationX, 20, 90);
                _camera.transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0);

            }
        }

        private void MoveCamera()
        {
            if (Input.GetMouseButton((int)MouseButton.MiddleMouseButton))
            {
                var x = Input.GetAxis("Mouse X") * _moveCoefficient * Time.deltaTime;
                var y = Input.GetAxis("Mouse Y") * _moveCoefficient * Time.deltaTime;

                _camera.transform.Translate(-x, -y, 0);
            }
        }

        private void ScrollCamera()
        {
            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            {
                var newY = _camera.transform.position.y - (Input.mouseScrollDelta.y * _scrollCoefficient * Time.deltaTime);
                newY = Mathf.Clamp(newY, 20, 300);
                _camera.transform.position = new Vector3(_camera.transform.position.x, newY, _camera.transform.position.z);
            }
        }
    }
}