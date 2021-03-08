public class SceneSaveData
{
    public int Id { get; set; }
    public Scene Scene { get; set; }
    public SceneType SceneType { get; set; }
    public CombatSceneSaveData CombatSceneSaveData { get; set; }

    public SerializableSceneSaveData Serialize()
    {
        return new SerializableSceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            SerializableCombatSceneSaveData = CombatSceneSaveData?.Serialize()
        };
    }
}
