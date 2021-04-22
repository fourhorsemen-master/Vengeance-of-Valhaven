using UnityEngine;

public class DevSaveDataManager : SaveDataManager
{
    protected override bool DestroyOnLoad => true;
    
    protected override void Awake()
    {
        base.Awake();
        Debug.LogWarning("Dev save data manager is active, this change should not be committed.");
    }
}
