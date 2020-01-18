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
            instanceLookup[template] = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }

    public List<Mortal> GetCollidingMortals(CollisionTemplate template, float scale, Vector3 position, Quaternion rotation)
    {
        Collider templateInstance = instanceLookup[template];
        templateInstance.transform.localScale = Vector3.one * scale;
        templateInstance.transform.position = position;
        templateInstance.transform.rotation = rotation;

        Mortal[] mortals = FindObjectsOfType<Mortal>();
        Collider[] mortalColliders = mortals.Select(a => a.GetComponent<Collider>()).ToArray();

        List<Mortal> collidingMortals = new List<Mortal>();

        foreach (var mc in mortals.Zip(mortalColliders, (m, c) => new { mortal = m, collider = c }))
        {
            if (Physics.ComputePenetration(
                mc.collider, mc.collider.transform.position, mc.collider.transform.rotation,
                templateInstance, templateInstance.transform.position, templateInstance.transform.rotation,
                out _, out _
            ))
            {
                collidingMortals.Add(mc.mortal);
            }
        }

        return collidingMortals;
    }
}
