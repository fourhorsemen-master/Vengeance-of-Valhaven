using UnityEngine;

public class LungeObject : StaticAbilityObject
{
    public AudioSource lungeSound = null;
    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = lungeSound.clip.length;
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        LungeObject prefab = AbilityObjectPrefabLookup.Instance.LungeObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}

