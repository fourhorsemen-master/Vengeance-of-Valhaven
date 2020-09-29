using UnityEngine;

public class RendObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitSound = null;
    
    public override float StickTime => 2f;

    public static RendObject Create(Transform transform, Vector3 position, bool enemiesHit)
    {
        RendObject rendObject = Instantiate(AbilityObjectPrefabLookup.Instance.RendObjectPrefab, position, Quaternion.LookRotation(Vector3.right), transform);

        if (enemiesHit) rendObject.PlayHitSound();

        return rendObject;
    }

    private void PlayHitSound()
    {
        hitSound.Play();
    }
}
