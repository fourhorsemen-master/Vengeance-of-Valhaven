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
            instanceLookup[template] = Instantiate(prefab, Vector3.down * float.MaxValue, Quaternion.identity);
        }
    }

    public List<Actor> GetCollidingActors(CollisionTemplate template, float scale, Vector3 position, Quaternion rotation)
    {
        MeshCollider templateInstance = instanceLookup[template];

        float currentScale = templateInstance.transform.localScale.magnitude;
        if (currentScale != scale)
        {
            templateInstance.transform.localScale = Vector3.one * scale;
            ResetMesh(templateInstance);
        }

        return RoomManager.Instance.ActorCache
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
        var temp = templateInstance.sharedMesh;
        templateInstance.sharedMesh = null;
        templateInstance.sharedMesh = temp;
    }
}
