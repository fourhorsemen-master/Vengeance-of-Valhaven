using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleLookup : Singleton<ModuleLookup>
{
    public ModuleDataLookup moduleDataLookup = new ModuleDataLookup(() => new ModuleDataListWrapper());

    public List<GameObject> GetPrefabsWithTags(SocketType socketType, List<ModuleTag> tags)
    {
        return moduleDataLookup[socketType].List
            .Where(moduleData => tags.All(t => moduleData.Tags.Contains(t)))
            .Select(moduleData => moduleData.Prefab)
            .ToList();
    }
}
