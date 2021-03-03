using UnityEngine;

public abstract class PersistentSingleton<T> : Singleton<T>, IPersistentObject where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        PersistenceManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        PersistenceManager.Instance.Deregister(this);
    }

    public abstract void Save(SaveData saveData);

    public abstract void Load(SaveData saveData);
}
