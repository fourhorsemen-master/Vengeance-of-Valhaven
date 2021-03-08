using System.Collections.Generic;
using System.Linq;

public class SceneSaveData
{
    public int Id { get; set; }
    public Scene Scene { get; set; }
    public SceneType SceneType { get; set; }
    public CombatSceneSaveData CombatSceneSaveData { get; set; }
    public Dictionary<int, int> SceneTransitionerIdToNextSceneId { get; set; }

    public SerializableSceneSaveData Serialize()
    {
        return new SerializableSceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            SerializableCombatSceneSaveData = CombatSceneSaveData?.Serialize(),
            SerializableSceneTransitioners = SceneTransitionerIdToNextSceneId?.Keys
                .Select(transitionerId => new SerializableSceneTransitioner
                {
                    SceneTransitionerId = transitionerId,
                    NextSceneId = SceneTransitionerIdToNextSceneId[transitionerId]
                })
                .ToList()
        };
    }
}
