using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableSceneSaveData
{
    [SerializeField] private int id;
    [SerializeField] private Scene scene;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private SerializableCombatSceneSaveData serializableCombatSceneSaveData;
    [SerializeField] private List<SerializableSceneTransitioner> serializableSceneTransitioners;

    public int Id {get => id; set => id = value; }
    public SceneType SceneType {get => sceneType; set => sceneType = value; }
    public Scene Scene {get => scene; set => scene = value; }
    public SerializableCombatSceneSaveData SerializableCombatSceneSaveData {get => serializableCombatSceneSaveData; set => serializableCombatSceneSaveData = value; }
    public List<SerializableSceneTransitioner> SerializableSceneTransitioners { get => serializableSceneTransitioners; set => serializableSceneTransitioners = value; }

    public SceneSaveData Deserialize()
    {
        return new SceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            CombatSceneSaveData = SerializableCombatSceneSaveData.Deserialize(),
            SceneTransitionerIdToNextSceneId = SerializableSceneTransitioners.ToDictionary(
                t => t.SceneTransitionerId,
                t => t.NextSceneId
            )
        };
    }
}
