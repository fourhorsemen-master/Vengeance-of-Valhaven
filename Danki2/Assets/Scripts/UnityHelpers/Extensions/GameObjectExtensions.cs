using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Sets the layer of the gameobject and all it's decendents to <param name="layer">. Ignores anything that already has a non-default layer set.
    /// </summary>
    /// <param name="object"></param>
    /// <param name="layer"></param>
    public static void SetLayerRecursively(this GameObject @object, Layer layer)
    {
        foreach (Transform trans in @object.GetComponentsInChildren<Transform>(true))
        {
            if (trans.gameObject.layer == (int)Layer.Default)
            {
                trans.gameObject.layer = (int)layer;
            }
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
