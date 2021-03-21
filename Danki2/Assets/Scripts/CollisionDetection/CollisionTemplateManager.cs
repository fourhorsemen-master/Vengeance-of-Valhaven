using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTemplateManager : Singleton<CollisionTemplateManager>
{
    public CollisionTemplateDictionary prefabLookup = new CollisionTemplateDictionary((MeshCollider)null);

    private readonly CollisionTemplateDictionary instanceLookup = new CollisionTemplateDictionary((MeshCollider)null);

    protected override void Awake()
    {
        base.Awake();

        foreach (CollisionTemplate template in Enum.GetValues(typeof(CollisionTemplate)))
        {
            MeshCollider prefab = prefabLookup[template];
            if (prefab == null)
            {
                Debug.LogError($"No prefab found for template: {template.ToString()}");
                continue;
            }
            instanceLookup[template] = Instantiate(prefab, Vector3.down * 100, Quaternion.identity);
        }
    }

    public List<Actor> GetCollidingActors(CollisionTemplate template, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        MeshCollider templateInstance = instanceLookup[template];

        Vector3 currentScale = templateInstance.transform.localScale;
        if (currentScale != scale)
        {
            templateInstance.transform.localScale = scale;
            ResetMesh(templateInstance);
        }

        return ActorCache.Instance.Cache
            .Where(actorCacheItem => Physics.ComputePenetration(
                actorCacheItem.Collider,
                actorCacheItem.Collider.transform.position,
                actorCacheItem.Collider.transform.rotation,
                templateInstance,
                position,
                rotation,
                out _,
                out _
            ))
            .Select(actorCacheItem => actorCacheItem.Actor)
            .ToList();
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
