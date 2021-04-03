using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTemplate : MonoBehaviour
{
    [SerializeField]
    MeshCollider meshCollider = null;

    private readonly HashSet<PhysicMaterial> materials = new HashSet<PhysicMaterial>();

    private CollisionSoundLevel collisionSoundLevel = default;

    private bool playCollisionSound = false;

    private void Start()
    {
        if (!playCollisionSound) Destroy(gameObject);

        // We have to wait for a physics cycle to run to ensure collisions have been registered
        this.WaitForFixedUpdateAndAct(() =>
        {
            CollisionSoundManager.Instance.Play(materials, collisionSoundLevel, transform.position);
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.sharedMaterial == null) return;

        materials.Add(other.sharedMaterial);
    }

    public void SetScale(Vector3 scale)
    {
        Vector3 currentScale = meshCollider.transform.localScale;
        if (currentScale != scale)
        {
            meshCollider.transform.localScale = scale;
            ResetMesh(meshCollider);
        }
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
