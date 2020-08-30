using System;
using UnityEngine;

public class FanOfKnivesObject : ProjectileObject
{
    [SerializeField]
    private AudioSource fireSound = null;

    [SerializeField]
    private AudioSource collisionSound = null;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation, bool playFireAudio = false)
    {
        FanOfKnivesObject fanOfKnivesObject = Instantiate(AbilityObjectPrefabLookup.Instance.FanOfKnivesObjectPrefab, position, rotation);

        fanOfKnivesObject.InitialiseProjectile(caster, collisionCallback, speed).SetSticky(2f);

        if (playFireAudio) fanOfKnivesObject.FireAudio();
    }

    private void FireAudio()
    {
        fireSound.Play();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<FanOfKnivesObject>() != null) return;

        base.OnTriggerEnter(other);

        if (other.gameObject != caster.gameObject) collisionSound.Play();
    }
}
