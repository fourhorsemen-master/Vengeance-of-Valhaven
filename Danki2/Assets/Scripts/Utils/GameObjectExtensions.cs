using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayer(this GameObject gameObject, Layer layer)
    {
        gameObject.layer = (int)layer;
    }
}
