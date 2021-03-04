using UnityEngine;

public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    private const int SaveDataVersion = 1;
    
    public SaveData SaveData { get; private set; }

    private void Start()
    {
        SaveData = SaveDataManager.Instance.TryLoad(out SaveData saveData) ? saveData : GenerateNewSaveData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneUtils.LoadScene(Scene.GameplayExitScene);
    }

    public void Save()
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
