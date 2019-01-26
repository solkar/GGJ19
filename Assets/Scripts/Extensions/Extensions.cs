using UnityEngine;

public static class Extensions
{
    public static Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
