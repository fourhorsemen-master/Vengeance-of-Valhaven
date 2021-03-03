using System.Collections.Generic;

public interface IPersistentObject
{
    void Save(SaveData saveData);
    void Load(SaveData saveData);
}

public class PersistenceManager : PersistentSingleton<PersistenceManager>
{
    public SaveData SaveData { get; private set; }

    private readonly List<IPersistentObject> persistentObjects = new List<IPersistentObject>();

    private void Start()
    {
        SaveData = SaveDataManager.Instance.TryLoad(out SaveData saveData) ? saveData : GetDefault();
    }

    public void Register(IPersistentObject persistentObject)
    {
        persistentObjects.Add(persistentObject);
    }

    public void DeRegister(IPersistentObject persistentObject)
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
