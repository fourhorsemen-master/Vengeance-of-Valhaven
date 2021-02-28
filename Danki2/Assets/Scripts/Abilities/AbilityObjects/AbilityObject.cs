using UnityEngine;

public abstract class AbilityObject : MonoBehaviour
{
    protected virtual void Start()
    {
        gameObject.layer = Layers.Abilities;
    }
}
