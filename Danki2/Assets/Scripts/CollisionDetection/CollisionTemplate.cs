using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTemplate : MonoBehaviour
{
    [SerializeField]
    MeshCollider meshCollider = null;

    private readonly HashSet<PhysicMaterial> collisionMaterials = new HashSet<PhysicMaterial>();

    private CollisionSoundLevel collisionSoundLevel = default;

    private bool playCollisionSound = false;
    private Actor owner = null;

    public static CollisionTemplate Create(CollisionTemplate prefab, Actor owner, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation)
            .Initialise(owner, scale);
    }

    private void Start()
    {
        if (!playCollisionSound) Destroy(gameObject);

        // We have to wait for a physics cycle to run to ensure collisions have been registered
        this.WaitForFixedUpdateAndAct(() =>
        {
            CollisionSoundManager.Instance.Play(collisionMaterials, collisionSoundLevel, transform.position);
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.sharedMaterial == null) return;

        if (owner.Colliders.Contains(other)) return;

        // Ignore the terrain
        if (other.gameObject.layer == (int)Layer.Floor) return;

        collisionMaterials.Add(other.sharedMaterial);
    }

    public List<Actor> GetCollidingActors()
    {
        return ActorCache.Instance.Cache
            .Where(actorCacheItem => Physics.ComputePenetration(
                actorCacheItem.Collider,
                actorCacheItem.Collider.transform.position,
                actorCacheItem.Collider.transform.rotation,
                meshCollider,
                meshCollider.transform.position,
                meshCollider.transform.rotation,
                out _,
                out _
            ))
            .Select(actorCacheItem => actorCacheItem.Actor)
            .ToList();
    }

    public void PlayCollisionSound(CollisionSoundLevel collisionSoundLevel)
    {
        this.collisionSoundLevel = collisionSoundLevel;
        playCollisionSound = true;
    }

    private CollisionTemplate Initialise(Actor owner, Vector3 scale)
    {
        this.owner = owner;

        Vector3 localScale = meshCollider.transform.localScale;
        localScale.x *= scale.x;
        localScale.y *= scale.y;
        localScale.z *= scale.z;
        meshCollider.transform.localScale = localScale;

        ResetMesh(meshCollider);

        return this;
    }

    /// <summary>
    /// After changing the scale of a transform, collider components take more than a frame to update. This method manually reapplies the collider mesh, forcing immediate recalculation of scale.
    /// </summary>
    /// <param name="templateInstance"></param>
    private void ResetMesh(MeshCollider templateInstance)
    {
        Mesh temp = templateInstance.sharedMesh;
        templateInstance.sharedMesh = null;
        templateInstance.sharedMesh = temp;
    }
}
