using UnityEngine;

public class WarningIfActive : MonoBehaviour
{
    [SerializeField] private string message = null;

    private void Awake()
    {
        Debug.LogWarning(message);
    }
}
