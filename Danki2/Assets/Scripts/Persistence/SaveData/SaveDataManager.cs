using System.Threading.Tasks;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private string serializedSaveData = null;
    private int saveThreadId = 0;

    public bool HasSaveData => serializedSaveData != null;
    public virtual BehaviourSubject<bool> SavingSubject { get; } = new BehaviourSubject<bool>(false);

    protected override bool DestroyOnLoad => false;

    public void Save(SaveData saveData)
    {
        SavingSubject.Next(true);
        saveThreadId++;
        Task.Run(() => SaveThreadStart(saveThreadId, saveData));
    }
    
    public SaveData Load() => JsonUtility.FromJson<SerializableSaveData>(serializedSaveData).Deserialize();

    public void Clear() => serializedSaveData = null;

    private void SaveThreadStart(int id, SaveData saveData)
    {
        string newSerializedSaveData = JsonUtility.ToJson(saveData.Serialize());
        if (id != saveThreadId) return;
        serializedSaveData = newSerializedSaveData;
        SavingSubject.Next(false);
    }
}
