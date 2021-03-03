using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    public SaveData SaveData { get; private set; }

    private readonly List<IPersistentObject> persistentObjects = new List<IPersistentObject>();

    private void Start()
    {
        SaveData = SaveDataManager.Instance.TryLoad(out SaveData saveData) ? saveData : GetDefault();

        GameplaySceneManager.Instance.GameplaySceneLoadedSubject.Subscribe(scene =>
        {
            persistentObjects.ForEach(o => o.Load(SaveData));
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
        SaveDataManager.Instance.Save(SaveData);
    }
    
    private SaveData GetDefault()
    {
        return new SaveData { CurrentScene = Scene.GameplayScene1 };
    }
}
