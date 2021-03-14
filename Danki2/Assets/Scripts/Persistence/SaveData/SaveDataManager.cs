using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private string serializedSaveData = null;

    public bool HasSaveData => serializedSaveData != null;

    protected override bool DestroyOnLoad => false;

    public void Save(SaveData saveData) => serializedSaveData = JsonUtility.ToJson(saveData.Serialize());
    
    public SaveData Load() => JsonUtility.FromJson<SerializableSaveData>(serializedSaveData).Deserialize();

    public void Clear() => serializedSaveData = null;
}
