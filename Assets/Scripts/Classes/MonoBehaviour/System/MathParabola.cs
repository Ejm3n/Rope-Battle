using UnityEngine;
using System;

public class MathHelper
{

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    public static Vector3 QuadraticBezierCurve(Vector3 from,Vector3 to,Vector3 handle,float time)
    {
        return (1.0f - time)* (1.0f - time) * from + 2.0f * (1.0f - time) * time * handle + time* time * to;
    }
}