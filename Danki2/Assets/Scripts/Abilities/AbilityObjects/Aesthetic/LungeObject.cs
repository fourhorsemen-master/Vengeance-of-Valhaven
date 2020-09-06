using System;
using UnityEngine;

public class LungeObject : StaticAbilityObject
{
    [SerializeField]
    private ModularPFXComponent modularPFX = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [SerializeField]
    private AudioSource hitAudioSource = null;

    public override float StickTime => 2f;

    public static LungeObject Create(Vector3 position, Quaternion rotation, Subject onFinishLunge, Transform parent)
    {
        LungeObject prefab = AbilityObjectPrefabLookup.Instance.LungeObjectPrefab;
        var lungeObject = Instantiate(prefab, position, rotation, parent);
        lungeObject.Setup(onFinishLunge);

        return lungeObject;
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }

    private void Setup(Subject onFinishLunge)
    {
        onFinishLunge.Subscribe(() =>
        {
            modularPFX.enabled = true;
            trailRenderer.emitting = false;
        });
    }
}

