using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableSaveData
{
    [SerializeField] private int version;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    [SerializeField] private int currentSceneId;
    [SerializeField] private int defeatSceneId;
    [SerializeField] private List<SerializableSceneSaveData> serializableSceneSaveDataList;
    [SerializeField] private List<SerializableSceneTransition> serializableSceneTransitions;
    
    public int Version { get => version; set => version = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }
    public int CurrentSceneId { get => currentSceneId; set => currentSceneId = value; }
    public int DefeatSceneId { get => defeatSceneId; set => defeatSceneId = value; }
    public List<SerializableSceneSaveData> SerializableSceneSaveDataList { get => serializableSceneSaveDataList; set => serializableSceneSaveDataList = value; }
    public List<SerializableSceneTransition> SerializableSceneTransitions { get => serializableSceneTransitions; set => serializableSceneTransitions = value; }

    public SaveData Deserialize()
    {
        return new SaveData
        {
            Version = Version,
            PlayerHealth = PlayerHealth,
            AbilityTree = SerializableAbilityTree.Deserialize(),
            CurrentSceneId = CurrentSceneId,
            DefeatSceneId = DefeatSceneId,
            SceneSaveDataLookup = SerializableSceneSaveDataList.ToDictionary(
                d => d.Id,
                d => d.Deserialize()
            ),
            SceneTransitions = SerializableSceneTransitions.ToDictionary(
                t => t.FromId,
                t => t.ToIds
            )
        };
    }
}
