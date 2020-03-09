using UnityEngine;

public class WhirlwindObject : StaticAbilityObject
{
    public AudioSource whirlwindSound = null;
    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = whirlwindSound.clip.length;
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        WhirlwindObject prefab = AbilityObjectPrefabLookup.Instance.WhirlwindObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
