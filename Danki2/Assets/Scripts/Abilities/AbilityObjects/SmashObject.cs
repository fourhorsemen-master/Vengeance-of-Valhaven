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

    public static void Create(Vector3 position, bool playSoundOnStart = true, float scaleFactor = 1f)
    {
        SmashObject prefab = AbilityObjectPrefabLookup.Instance.SmashObjectPrefab;
        SmashObject smashObject = Instantiate(prefab, position, Quaternion.identity);
        smashObject.gameObject.transform.localScale *= scaleFactor;
        smashObject.playSoundOnStart = playSoundOnStart;
    }
}
