using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    public SaveData SaveData { get; private set; }

    private readonly List<IPersistentObject> persistentObjects = new List<IPersistentObject>();

    private void Start()
    {
        if (SaveDataManager.Instance.TryLoad(out SaveData saveData))
        {
            SaveData = saveData;
        }
        else
        {
            SaveData = GenerateNewSaveData();
        }

        GameplaySceneManager.Instance.GameplaySceneLoadedSubject.Subscribe(gameplayScene =>
        {
            SceneLoadedManager.Instance.SceneLoadedSubject.Subscribe(() =>
            {
                persistentObjects.ForEach(o => o.Load(SaveData));
            });
        });

        GameplaySceneManager.Instance.GameplaySceneExitedSubject.Subscribe(gameplayScene =>
        {
            persistentObjects.ForEach(o => o.Save(SaveData));
            Save();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneUtils.LoadScene(Scene.GameplayExitScene);
    }

    public void Register(IPersistentObject persistentObject)
    {
        persistentObjects.Add(persistentObject);
    }

    public void Deregister(IPersistentObject persistentObject)
    {
        persistentObjects.Remove(persistentObject);
    }

    public void Save()
    {
        SaveData.PlayerHealth = RoomManager.Instance.Player.HealthManager.Health;
        SaveDataManager.Instance.Save(SaveData);
    }
    
    private SaveData GenerateNewSaveData()
    {
        return new SaveData(Scene.GameplayScene1, 100);
    }
}
