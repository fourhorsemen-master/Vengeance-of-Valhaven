using System.Collections;
using UnityEngine;

public abstract class StaticAbilityObject : MonoBehaviour
{
    public abstract float StickTime { get; set; }

    protected virtual void Start()
    {
        StartCoroutine(DissapearAfter(StickTime));
    }

    private IEnumerator DissapearAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
