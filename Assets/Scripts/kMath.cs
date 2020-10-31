using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class kMath
{
    public static float AngleBetween(Vector2 a, Vector2 b)
    {
        a.Normalize();
        b.Normalize();
        float dot = Vector2.Dot(a, b);
        dot = Mathf.Clamp(dot, -1, 1);
        return Mathf.Rad2Deg * Mathf.Acos(dot);
    }
}
