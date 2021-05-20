using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleLookup : Singleton<ModuleLookup>
{
    [SerializeField]
    public ModuleDataLookup moduleDataLookup = new ModuleDataLookup(() => new SocketData());

    public List<ModuleData> GetModuleDataWithMatchingTags(
        Zone zone,
        SocketType socketType,
        List<ModuleTag> tags,
        List<ModuleTag> tagsToExclude
    ) => moduleDataLookup[socketType].ModuleData
            .Where(moduleData => moduleData.Zones.Contains(zone))
            .Where(moduleData => tags.All(t => moduleData.Tags.Contains(t)))
            .Where(moduleData => tagsToExclude.All(t => !moduleData.Tags.Contains(t)))
            .ToList();

    public SocketRotationType GetSocketRotationType(SocketType socketType) => moduleDataLookup[socketType].SocketRotationType;
}
