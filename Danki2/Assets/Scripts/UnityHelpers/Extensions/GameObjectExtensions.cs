using UnityEngine;

public static class GameObjectExtensions
{
    public static bool TryGetComponent<T>(this GameObject @object, out T component)
        where T : MonoBehaviour
    {
        component = @object.GetComponent<T>();
        return component != null;
    }

    public static bool IsActor(this GameObject gameObject)
    {
        return gameObject.tag == Tags.Player || gameObject.tag == Tags.Enemy;
    }
}
