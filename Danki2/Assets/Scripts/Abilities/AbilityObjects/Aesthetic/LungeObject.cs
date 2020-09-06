using UnityEngine;

public class LungeObject : StaticAbilityObject
{
    [SerializeField]
    private ModularPFXComponent modularPFX = null;

    [SerializeField]
    private AudioSource hitAudioSource = null;

    [SerializeField]
    private AudioSource swingAudioSource = null;

    public override float StickTime => 2f;

    public static LungeObject Create(Vector3 position, Quaternion rotation, Subject<Vector3> onFinishMovement)
    {
        LungeObject prefab = AbilityObjectPrefabLookup.Instance.LungeObjectPrefab;
        var lungeObject = Instantiate(prefab, position, rotation);
        lungeObject.Setup(onFinishMovement);

        return lungeObject;
    }

    public void PlaySwingSound()
    {
        swingAudioSource.Play();
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }

    private void Setup(Subject<Vector3> onFinishMovement)
    {
        onFinishMovement.Subscribe(position =>
        {
            transform.position = position;
            modularPFX.enabled = true;
        });
    }
}

