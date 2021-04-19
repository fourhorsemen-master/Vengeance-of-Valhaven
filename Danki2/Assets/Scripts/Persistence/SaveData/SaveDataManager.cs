using System.Threading;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private string serializedSaveData = null;

    public bool HasSaveData => serializedSaveData != null;
    public BehaviourSubject<bool> SavingSubject { get; } = new BehaviourSubject<bool>(false);

    protected override bool DestroyOnLoad => false;

    public void Save(SaveData saveData)
    {
        SavingSubject.Next(true);
        new Thread(SaveThreadStart).Start(saveData);
    }
    
    public SaveData Load() => JsonUtility.FromJson<SerializableSaveData>(serializedSaveData).Deserialize();

    public void Clear() => serializedSaveData = null;

    private void SaveThreadStart(object saveData)
    {
        serializedSaveData = JsonUtility.ToJson(((SaveData) saveData).Serialize());
        SavingSubject.Next(false);
    }
}
