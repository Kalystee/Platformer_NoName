using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector3 ToVector3(this Vector2 vec)
    {
        return new Vector3(vec.x, vec.y);
    }

    public static Vector2 ClampEach(this Vector2 vec, float min, float max)
    {
        return new Vector2(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max));
    }

    public static Vector3 ClampEach(this Vector3 vec, float min, float max)
    {
        return new Vector3(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max), Mathf.Clamp(vec.z, min, max));
    }
}
