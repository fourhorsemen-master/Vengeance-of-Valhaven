using UnityEngine;

public class DashObject : StaticAbilityObject
{
    public AudioSource rollSound = null;

    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = rollSound.clip.length;
    }

    public static void Create(Transform casterTransform)
    {
        DashObject prefab = AbilityObjectPrefabLookup.Instance.DashObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
