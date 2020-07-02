using UnityEngine;

public static class QuaternionExtension
{
    public static Quaternion Backwards(this Quaternion quaternion)
    {
        return quaternion * Quaternion.Euler(0f, 180f, 0f);
    }
}
