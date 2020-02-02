using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTemplateManager : Singleton<CollisionTemplateManager>
{
    public CollisionTemplateDictionary prefabLookup = new CollisionTemplateDictionary((Collider)null);

    private readonly CollisionTemplateDictionary instanceLookup = new CollisionTemplateDictionary((Collider)null);

    protected override void Awake()
    {
        base.Awake();

        foreach (CollisionTemplate template in Enum.GetValues(typeof(CollisionTemplate)))
        {
            Collider prefab = prefabLookup[template];
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
        Collider templateInstance = instanceLookup[template];
        templateInstance.transform.localScale = Vector3.one * scale;

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
}
