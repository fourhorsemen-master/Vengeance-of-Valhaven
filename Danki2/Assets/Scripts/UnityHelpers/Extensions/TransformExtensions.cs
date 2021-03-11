using UnityEngine;

public static class TransformExtensions
{
    public static float DistanceFromPlayer(this Transform transform)
    {
        return Vector3.Distance(transform.position, ActorCache.Instance.Player.transform.position);
    }
}
