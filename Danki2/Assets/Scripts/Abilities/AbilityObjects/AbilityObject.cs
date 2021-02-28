using UnityEngine;

public abstract class AbilityObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.SetLayerRecursively(Layer.Abilities);
    }
}
