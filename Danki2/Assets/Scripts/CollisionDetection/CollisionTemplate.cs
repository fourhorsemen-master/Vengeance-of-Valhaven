using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTemplate : MonoBehaviour
{
    [SerializeField]
    MeshCollider collider;

    public void SetScale(Vector3 scale)
    {
        Vector3 currentScale = collider.transform.localScale;
        if (currentScale != scale)
        {
            collider.transform.localScale = scale;
            ResetMesh(collider);
        }
    }

    public List<Actor> GetCollidingActors()
    {
        return ActorCache.Instance.Cache
            .Where(actorCacheItem => Physics.ComputePenetration(
                actorCacheItem.Collider,
                actorCacheItem.Collider.transform.position,
                actorCacheItem.Collider.transform.rotation,
                collider,
                collider.transform.position,
                collider.transform.rotation,
                out _,
                out _
            ))
            .Select(actorCacheItem => actorCacheItem.Actor)
            .ToList();
    }

    public void PlayCollisionSounds(int impactSize)
    {

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
