using System;
using UnityEngine;

public class SwordThrowObject : ProjectileObject
{
    [SerializeField]
    private AudioSource collisionSound = null;

    [SerializeField]
    private GameObject mesh = null;

    private bool collided;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        SwordThrowObject prefab = AbilityObjectPrefabLookup.Instance.SwordThrowObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky(2f);
    }

    protected override void Update()
    {
        base.Update();

        if (!collided)
        {
            mesh.transform.RotateAround(transform.position, Vector3.up, 20f);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject != caster.gameObject)
        {
            collided = true;
            collisionSound.Play();
        }
    }
}
