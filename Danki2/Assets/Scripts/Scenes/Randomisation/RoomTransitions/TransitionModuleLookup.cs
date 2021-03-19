using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionModuleLookup : Singleton<TransitionModuleLookup>
{
    [SerializeField]
    public List<TransitionModuleData> transitionModuleDataList = new List<TransitionModuleData>();

    public List<GameObject> GetExitModulesWithMatchingTags(List<ModuleTag> tags) => transitionModuleDataList
        .Where(d => d.Type == TransitionModuleType.Exit)
        .Where(d => tags.All(t => d.Tags.Contains(t)))
        .Select(d => d.Prefab)
        .ToList();

    public List<GameObject> GetBlockersWithMatchingTags(List<ModuleTag> tags) => transitionModuleDataList
        .Where(d => d.Type == TransitionModuleType.Blocker)
        .Where(d => tags.All(t => d.Tags.Contains(t)))
        .Select(d => d.Prefab)
        .ToList();
}
