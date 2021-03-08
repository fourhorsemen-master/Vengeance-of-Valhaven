using System;
using UnityEngine;

[Serializable]
public class SerializableSceneSaveData
{
    [SerializeField] private int id;
    [SerializeField] private Scene scene;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private SerializableCombatSceneSaveData serializableCombatSceneSaveData;

    public int Id {get => id; set => id = value; }
    public SceneType SceneType {get => sceneType; set => sceneType = value; }
    public Scene Scene {get => scene; set => scene = value; }
    public SerializableCombatSceneSaveData SerializableCombatSceneSaveData {get => serializableCombatSceneSaveData; set => serializableCombatSceneSaveData = value; }

    public SceneSaveData Deserialize()
    {
        return new SceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            CombatSceneSaveData = SerializableCombatSceneSaveData?.Deserialize()
        };
    }
}
