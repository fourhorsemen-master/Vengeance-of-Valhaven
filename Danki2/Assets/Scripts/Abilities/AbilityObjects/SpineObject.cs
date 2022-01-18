using System;
using UnityEngine;

public class SpineObject : AbilityObject
{
    [SerializeField]
    private Collider spineCollider = null;

    [SerializeField]
    private Rigidbody rigidBody = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    private const float Speed = 6f;
    private const float StickTime = 5f;

    protected Actor caster;
    private Action<GameObject> collisionCallback;
    private bool stuck;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, Vector3 position, Quaternion rotation)
    {
        SpineObject spineObject = Instantiate(AbilityObjectPrefabLookup.Instance.SpineObjectPrefab, position, rotation);

        spineObject.caster = caster;
        spineObject.collisionCallback = collisionCallback;
    }

    protected virtual void Update()
    {
        if (!stuck)
        {
            transform.position += Speed * Time.deltaTime * transform.forward;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != caster.gameObject)
        {
            trailRenderer.emitting = false;
        }

        if (ReferenceEquals(caster.gameObject, other.gameObject)) return;

        collisionCallback(other.gameObject);

        if (other.sharedMaterial != null)
        {
            CollisionSoundManager.Instance.Play(other.sharedMaterial, CollisionSoundLevel.Low, transform.position);
        }

        StickTo(other.transform);
    }

    private void StickTo(Transform newParent)
    {
        stuck = true;

        Destroy(rigidBody);
        Destroy(spineCollider);
        transform.SetParent(newParent);

        this.WaitAndAct(StickTime, () => Destroy(gameObject));
    }
}
