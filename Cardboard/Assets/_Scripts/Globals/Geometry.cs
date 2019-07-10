using UnityEngine;

static public class Geometry
{
    static public Vector3 PointLineIntersection(Vector3 P, Vector3 A, Vector3 B)
    {
        Vector3 u = (B - A).normalized;
        Vector3 D = u * Vector2.Dot(P - A, u);
        Vector3 Q = A + D;

        float anglePAB = Mathf.Acos(Vector3.Dot(P - A, B - A) / ((P - A).magnitude * (B - A).magnitude));
        float anglePBA = Mathf.Acos(Vector3.Dot(P - B, A - B) / ((P - B).magnitude * (A - B).magnitude));

        if (anglePAB > Mathf.PI / 2) Q = A;
        if (anglePBA > Mathf.PI / 2) Q = B;

        return Q;
    }

    static public bool LineCircleIntersection(Vector2 A, Vector2 B, Vector2 P, float R)
    {
        Vector3 Q = Geometry.PointLineIntersection(P, A, B);
        float h = Vector3.Distance(P, Q);

        if (h <= R)
        {
            return true;
        }
        return false;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
