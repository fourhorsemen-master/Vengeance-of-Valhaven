using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
