using UnityEngine;

public class DevSaveDataManager : SaveDataManager
{
    public override BehaviourSubject<bool> SavingSubject { get; } = new BehaviourSubject<bool>(false);

    protected override bool DestroyOnLoad => true;
    
    protected override void Awake()
    {
        base.Awake();
        Debug.LogWarning("Dev save data manager is active, this change should not be committed.");
    }
}
