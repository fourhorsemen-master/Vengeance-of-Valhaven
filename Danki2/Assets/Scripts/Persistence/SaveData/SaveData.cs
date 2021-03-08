using System.Collections.Generic;
using System.Linq;

public class SaveData
{
    public int Version { get; set; }

    public int PlayerHealth { get; set; }
    public AbilityTree AbilityTree { get; set; }

    public int CurrentSceneId { get; set; }
    public int DefeatSceneId { get; set; }
    public Dictionary<int, SceneSaveData> SceneSaveDataLookup { get; set; }
    public Dictionary<int, List<int>> SceneTransitions { get; set; }

    public SerializableSaveData Serialize()
    {
        return new SerializableSaveData
        {
            Version = Version,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = new SerializableAbilityTree(AbilityTree),
            CurrentSceneId = CurrentSceneId,
            DefeatSceneId = DefeatSceneId,
            SerializableSceneSaveDataList = SceneSaveDataLookup.Values
                .Select(sceneData => sceneData.Serialize())
                .ToList(),
            SerializableSceneTransitions = SceneTransitions.Keys
                .Select(fromId => new SerializableSceneTransition
                {
                    FromId = fromId,
                    ToIds = SceneTransitions[fromId]
                })
                .ToList()
        };
    }
}
