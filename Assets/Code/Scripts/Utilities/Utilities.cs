using UnityEngine;
using UnityEngine.UIElements;

namespace QuantumRevenant.Utilities
{
    public static class Utility
    {
        public static float NormalizeAngle(float angle, bool equalLimits = false)
        {
            if (angle % 360 != 0 || equalLimits)
                return angle %= 360;

            return angle == 0 ? 0 : 360;
        }
        public static bool DoesTagExist(string aTag)
        {
            try
            {
                GameObject.FindGameObjectWithTag(aTag);
                return true;
            }
            catch
            {
                Debug.Log($"The tag {aTag} does not exist!");
                return false;
            }
        }
        public static Vector2 RotatePointRelativeToPivot(Vector2 Point, Vector2 Pivot, float EulerAngle)
        {
            Quaternion angle = Quaternion.Euler(new Vector3(0, 0, EulerAngle));
            return RotatePointRelativeToPivot(Point, Pivot, angle);
        }

        public static Vector3 RotatePointRelativeToPivot(Vector3 Point, Vector3 Pivot, Quaternion Rotation)
        {
            Vector3 OriginalVector = Point - Pivot;
            Vector3 Offset = Rotation * OriginalVector;

            return Offset + Pivot;
        }

        public static BoxCollider2D ResizeBoxCollider2D(SpriteRenderer renderer, Collider2D collider2D, Vector3 scale, float resizeFactor = 0.5f)
        {
            Vector3 v = renderer.bounds.size * resizeFactor;

            v.x /= scale.x;
            v.y /= scale.y;
            v.z /= scale.z;

            BoxCollider2D b = collider2D as BoxCollider2D;

            b.size = v;

            return b;
        }
        public static string GetVersion() { return Application.version; }
    }

}

