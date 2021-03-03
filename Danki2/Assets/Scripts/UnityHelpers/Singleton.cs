using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogError($"Tried to instantiate more than one instance of the singleton: {typeof(T).Name}");
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
