using UnityEngine;

public class RollObject : StaticAbilityObject
{
    public AudioSource rollSound = null;
    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = rollSound.clip.length;
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        RollObject prefab = AbilityObjectPrefabLookup.Instance.RollObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
