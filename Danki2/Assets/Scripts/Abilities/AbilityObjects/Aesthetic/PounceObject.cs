using UnityEngine;

public class PounceObject : StaticAbilityObject
{
    public AudioSource pounceSound = null;

    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = pounceSound.clip.length;
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        PounceObject prefab = AbilityObjectPrefabLookup.Instance.PounceObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}

