using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleLookup : Singleton<ModuleLookup>
{
    [SerializeField]
    public ModuleDataLookup moduleDataLookup = new ModuleDataLookup(() => new SocketData());

    public List<ModuleData> GetModuleDataWithMatchingTags(SocketType socketType, List<ModuleTag> tags) =>
        moduleDataLookup[socketType].ModuleData
            .Where(moduleData => tags.All(t => moduleData.Tags.Contains(t)))
            .ToList();

    public SocketRotationType GetSocketRotationType(SocketType socketType) => moduleDataLookup[socketType].SocketRotationType;
}
