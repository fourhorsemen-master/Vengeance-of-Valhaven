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
        if (Input.GetKeyDown(KeyCode.Escape)) SaveAndQuit();
    }

    public void Save()
    {
        SaveData = new SaveData(
            SaveDataVersion,
            GameplaySceneManager.Instance.CurrentScene,
            RoomManager.Instance.Player.HealthManager.Health
        );
        SaveDataManager.Instance.Save(SaveData);
    }

    private SaveData GenerateNewSaveData()
    {
        return new SaveData(SaveDataVersion, Scene.GameplayScene1, 100);
    }

    private void SaveAndQuit()
    {
        Save();
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
