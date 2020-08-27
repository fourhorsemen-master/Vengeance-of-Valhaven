using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource leapSound = null;

    [SerializeField]
    private AudioSource momentumSound = null;

    private float duration;

    public override float StickTime => Mathf.Max(leapSound.clip.length + momentumSound.clip.length, duration + momentumSound.clip.length);

    public static LeapObject Create(Transform casterTransform, float duration)
    {
        LeapObject leapObject = Instantiate(AbilityObjectPrefabLookup.Instance.LeapObjectPrefab, casterTransform);
        leapObject.duration = duration;
        return leapObject;
    }

    public void PlayMomentumSound()
    {
        leapSound.Stop();
        momentumSound.Play();
    }
}
