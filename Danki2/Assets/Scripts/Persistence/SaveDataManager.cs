using UnityEngine;

public class SaveDataManager : NotDestroyedOnLoadSingleton<SaveDataManager>
{
    private string serializedSaveData = null;

    public bool HasSaveData => serializedSaveData != null;

    public void Save(SaveData saveData) => serializedSaveData = JsonUtility.ToJson(saveData);
    
    public SaveData Load() => JsonUtility.FromJson<SaveData>(serializedSaveData);

    public void Clear() => serializedSaveData = null;
}
