using UnityEngine;

public class SmashObject : StaticAbilityObject
{
    public AudioSource smashSound = null;

    private bool playSoundOnStart = true;

    public override float StickTime => smashSound.clip.length;

    protected override void Start()
    {
        base.Start();
    
        if (playSoundOnStart) smashSound.Play();
    }

    public static SmashObject Create(Vector3 position)
    {
        SmashObject prefab = AbilityObjectPrefabLookup.Instance.SmashObjectPrefab;
        return Instantiate(prefab, position, Quaternion.identity);
    }

    public static SmashObject CreateWithoutSound(Vector3 position)
    {
        SmashObject smashObject = Create(position);
        smashObject.playSoundOnStart = false;
        return smashObject;
    }
}
