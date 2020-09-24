using UnityEngine;

public static class GameObjectExtensions
{
    public static bool MatchesId(this GameObject @object, GameObject other)
    {
        return @object.GetInstanceID() == other.GetInstanceID();
    }

    public static bool TryGetComponent<T>(this GameObject @object, out T component)
        where T : MonoBehaviour
    {
        component = @object.GetComponent<T>();
        return component != null;
    }
}
