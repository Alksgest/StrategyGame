using UnityEngine;

using StrategyGame.Assets.Scripts.Building;
using System;

namespace StrategyGame.Assets.Scripts.Unit
{
    public class UnitController : MonoBehaviour
    {
        public bool Selected { get; private set; } = false;

        [SerializeField]
        private float _speed = 0.01f;

        [SerializeField]
        private Material _defaultMaterial;
        [SerializeField]
        private Material _selectedMaterial;

        private bool _isMoving = false;
        private Vector3 _pointToMove;
        private Vector3 _pointToRotate;

        public GameObject ObjectAttachedTo { get; set; }

        private bool _canMove = false;
        private bool _isRotating = true;

        private void FixedUpdate()
        {
            if (_isMoving) Move();
            if (_isRotating) Rotate();
        }

        public void Select()
        {
            Selected = !Selected;
            var renderer = GetComponentInChildren<MeshRenderer>();
            renderer.material = Selected ? _selectedMaterial : _defaultMaterial;
        }

        public void AskToMove(Vector3 point)
        {
            // Debug.Log(point);
            // Debug.Log(this.transform.position);

            _pointToMove = point;
            _pointToRotate = point;

            _pointToMove.y = this.transform.position.y;

            _isMoving = true;
            _isRotating = true;

            if (this.tag == "AttachedToMineUnit")
            {
                if (ObjectAttachedTo != null)
                {
                    ObjectAttachedTo.GetComponent<MineEdgeController>().DeatachUnit();
                }
            }
        }

        private void Rotate()
        {
            if (_pointToRotate != null)
            {
                var targetRotation = Quaternion.LookRotation(_pointToRotate - transform.position);
                var angles = targetRotation.eulerAngles;
                angles.y += 180;
                targetRotation.eulerAngles = angles;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

                if(Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 2)
                {
                    _isRotating = false;
                }
            }
        }

        private void Move()
        {
            if (_canMove)
            {
                var delta = _pointToMove - transform.position;
                delta.Normalize();
                transform.position = transform.position + (delta * _speed * Time.deltaTime);

                var vec = this.transform.position - _pointToMove;
                if (Mathf.Abs(vec.x) <= 0.1 && Mathf.Abs(vec.z) <= 0.1)
                    _isMoving = false;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Terrain" && !_canMove)
            {
                _canMove = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.tag == "Terrain" && _canMove)
            {
                _canMove = false;
            }
        }
    }
}

//  private void __RotateToPoint(Vector3 point)
//         {
//             var curentPosition = this.transform.position;

//             int direction = GetAngleBase(point, curentPosition);

//             var distance = Mathf.Sqrt(Mathf.Pow(point.x - curentPosition.x, 2) + Mathf.Pow(point.z - curentPosition.z, 2));
//             var angle = Mathf.Acos(Mathf.Abs(curentPosition.z - point.z) / distance) * 180;

//             // var angle = 0;
//             Debug.Log("Angle : " + angle);
//             Debug.Log("Direction : " + direction);

//             switch (direction)
//             {
//                 case 0:
//                 case 180:
//                     angle = angle + 0;
//                     break;
//                 case 90:
//                     angle = 90 + (90 - angle);
//                     break;
//                 case 270:
//                     angle = 270 + (90 - angle);
//                     break;
//             }

//             this.transform.eulerAngles = new Vector3(0, angle, 0);

//             Debug.Log("Angle of rotation after : " + angle);
//         }

//         private float GetAngleToRotate(Vector3 pointA, Vector3 pointB)
//         {
//             Vector3 pointC = new Vector3(pointB.x, 0, pointA.z);

//             Vector3 vectorAB = new Vector3(pointB.x - pointA.x, 0, pointB.z - pointA.z);
//             Vector3 vectorAC = new Vector3(pointC.x - pointA.x, 0, pointC.z - pointA.z);

//             Console.WriteLine(vectorAB);
//             Console.WriteLine(vectorAC);

//             var scalar = vectorAB.x * vectorAC.x + vectorAB.z * vectorAC.z;

//             var absVectorAB = Mathf.Sqrt(vectorAB.x * vectorAB.x + vectorAB.z * vectorAB.z);
//             var absVectorAC = Mathf.Sqrt(vectorAC.x * vectorAC.x + vectorAC.z * vectorAC.z);

//             var cos = scalar / (absVectorAB * absVectorAC);
//             var angle = Mathf.Acos(cos);


//             return angle;
//         }

//         private int GetAngleBase(Vector3 pointA, Vector3 pointB)
//         {
//             var z1 = pointB.z;
//             var z2 = pointA.z;
//             var x1 = pointB.x;
//             var x2 = pointA.x;

//             var angleBase = z1 > z2 && x1 > x2 ? 0 :
//                             z1 < z2 && x1 > x2 ? 90 :
//                             z1 < z2 && x1 < x2 ? 180 : 270;

//             return angleBase;
//         }
