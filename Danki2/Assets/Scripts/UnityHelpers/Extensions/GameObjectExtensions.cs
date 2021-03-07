using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayerRecursively(this GameObject @object, Layer layer)
    {
        foreach (Transform trans in @object.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = (int)layer;
        }
    }

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

    public static void SetTag(this GameObject @object, Tag tag)
    {
        @object.tag = tag.ToString();
    }
}
