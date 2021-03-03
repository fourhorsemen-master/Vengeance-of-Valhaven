using UnityEngine;

public class SaveDataManager : PersistentSingleton<SaveDataManager>
{
    private string serializedSaveData = null;

    public bool HasSaveData => serializedSaveData != null;

    public void Save(SaveData saveData)
    {
        serializedSaveData = JsonUtility.ToJson(saveData);
    }
    
    public bool TryLoad(out SaveData saveData)
    {
        if (HasSaveData)
        {
            saveData = JsonUtility.FromJson<SaveData>(serializedSaveData);
            return true;
        }

        saveData = null;
        return false;
    }

    public void Clear()
    {
        serializedSaveData = null;
    }
}
