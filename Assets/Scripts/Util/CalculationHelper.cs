using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class CalculationHelper
    {
        public static Vector2 CalculateRectangleSize(Vector3 left, Vector3 right)
        {
            if (left.x < right.x)
            {
                var temp = right;
                right = left;
                left = temp;
            }
            
            var lN = new Vector3(right.x, left.y, left.z);
            var rN = new Vector3(left.x, right.y, right.z);

            var lrN = new Vector3(left.x, left.y, right.z); ;
            var rlN = new Vector3(right.x, right.y, left.z); ;

            var width = Mathf.Sqrt(Mathf.Pow(lN.x - rN.x, 2) + Mathf.Pow(lN.y - rN.y, 2) + Mathf.Pow(lN.z - rN.z, 2));
            var height = Mathf.Sqrt(Mathf.Pow(lN.x - rN.x, 2) + Mathf.Pow(lN.y - rN.y, 2) + Mathf.Pow(lN.z - rN.z, 2));

            return new Vector2(width, height);
        }

        public static Vector3 CalculateRectanglePosition(Vector3 start, Vector2 size)
        {
            var deltaX = size.x / 2 * -1;
            var deltaZ = size.y / 2;

            return new Vector3(start.x + deltaX, 5, start.z + deltaZ);
        }

        public static bool IsInArea(Vector3 objectPosition, Vector3 areaPosition, Vector2 areaSize)
        {
            var op = objectPosition;
            var ap = areaPosition;
            var s = areaSize;
            bool x = op.x <= ap.x + s.x && op.x >= ap.x;
            bool z = op.z <= ap.z && op.z >= ap.z - s.y;

            return x && z;
        }
    }
}