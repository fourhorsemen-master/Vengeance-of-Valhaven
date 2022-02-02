using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float time = 0;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
