using UnityEngine;

public abstract class StaticAbilityObject : MonoBehaviour
{
    public abstract float StickTime { get; }

    protected virtual void Start()
    {
        this.WaitAndAct(StickTime, () => Destroy(gameObject));
    }
}
