using UnityEngine;
using System.Collections;

public static class Vector3Ex
{
    public static Vector3 WithZ(this Vector3 vec, float Z = 0)
    {
        vec.z = Z;
        return vec;
    }
}
