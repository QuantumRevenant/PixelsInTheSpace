using System.Drawing;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
        public static Vector2 RotatePointRelativeToPivot(Vector2 Point, Vector2 Pivot, float Angle)
        {
            Angle *= Mathf.Deg2Rad;

            float s = Mathf.Sin(Angle);
            float c = Mathf.Cos(Angle);

            Point -= Pivot;

            Vector2 output;

            output.x = Point.x * c - Point.y * s;
            output.y = Point.x * s + Point.y * c;

            output += Pivot;

            return output;
        }
        public static Vector3 RotatePoint3DRelativeToPivotZ(Vector3 Point, Vector3 Pivot, float Angle)
        {
            Vector2 Point2D = new Vector2(Point.x, Point.y);
            Vector2 Pivot2D = new Vector2(Pivot.x, Pivot.y);

            Vector2 Output2D = RotatePointRelativeToPivot(Point2D, Pivot2D, Angle);

            Point.x = Output2D.x;
            Point.y = Output2D.y;

            return Point;
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
        public static string GetVersion(){return Application.version;}
    }

}

