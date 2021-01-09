using UnityEngine;

public class SwipeObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    public override float StickTime => 5f;

    public static SwipeObject Create(Vector3 position, Quaternion rotation)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.SwipeObjectPrefab, position, rotation);
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}