using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleLookup : Singleton<ModuleLookup>
{
    [SerializeField]
    public ModuleDataLookup moduleDataLookup = new ModuleDataLookup(() => new SocketData());

    public List<GameObject> GetModulesWithMatchingTags(SocketType socketType, List<ModuleTag> tags)
    {
        return moduleDataLookup[socketType].ModuleData
            .Where(moduleData => tags.All(t => moduleData.Tags.Contains(t)))
            .Select(moduleData => moduleData.Prefab)
            .ToList();
    }
}
