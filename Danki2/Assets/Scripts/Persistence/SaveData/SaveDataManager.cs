using System.Threading;
using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    private string serializedSaveData = null;
    private int saveThreadId = 0;

    public bool HasSaveData => serializedSaveData != null;
    public BehaviourSubject<bool> SavingSubject { get; } = new BehaviourSubject<bool>(false);

    protected override bool DestroyOnLoad => false;

    public void Save(SaveData saveData)
    {
        SavingSubject.Next(true);
        saveThreadId++;
        new Thread(SaveThreadStart).Start(new SaveThreadContext(saveThreadId, saveData));
    }
    
    public SaveData Load() => JsonUtility.FromJson<SerializableSaveData>(serializedSaveData).Deserialize();

    public void Clear() => serializedSaveData = null;

    private void SaveThreadStart(object o)
    {
        SaveThreadContext saveThreadContext = (SaveThreadContext) o;

        string newSerializedSaveData = JsonUtility.ToJson(saveThreadContext.SaveData.Serialize());
        if (saveThreadContext.Id != saveThreadId) return;
        serializedSaveData = newSerializedSaveData;
        SavingSubject.Next(false);
    }
    
    private struct SaveThreadContext
    {
        public int Id { get; }
        public SaveData SaveData { get; }

        public SaveThreadContext(int id, SaveData saveData)
        {
            Id = id;
            SaveData = saveData;
        }
    }
}
