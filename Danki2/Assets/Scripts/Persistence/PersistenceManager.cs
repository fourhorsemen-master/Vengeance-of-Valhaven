public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    private const int SaveDataVersion = 1;
    
    public virtual SaveData SaveData { get; private set; }

    protected virtual void Start()
    {
        if (SaveDataManager.Instance.HasSaveData)
        {
            SaveData = SaveDataManager.Instance.Load();
            return;
        }

        SaveData = GenerateNewSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    public virtual void Save()
    {
        SaveData = new SaveData(
            SaveDataVersion,
            GameplaySceneManager.Instance.CurrentScene,
            RoomManager.Instance.Player.HealthManager.Health,
            new SerializableAbilityTree(RoomManager.Instance.Player.AbilityTree)
        );
        SaveDataManager.Instance.Save(SaveData);
    }

    private SaveData GenerateNewSaveData()
    {
        AbilityTree abilityTree = AbilityTreeFactory.CreateTree(
            new EnumDictionary<AbilityReference, int>(3),
            AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );
        
        return new SaveData(SaveDataVersion, Scene.GameplayScene1, 100, new SerializableAbilityTree(abilityTree));
    }
}
