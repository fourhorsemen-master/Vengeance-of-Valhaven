using UnityEngine;

public class RendObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource audioSource = null;
    
    public override float StickTime => audioSource.clip.length;

    public static RendObject Create(Transform transform)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.RendObjectPrefab, transform);
    }
}
