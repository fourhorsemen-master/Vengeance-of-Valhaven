using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    [SerializeField]
    private Wolf wolfPrefab = null;

    [SerializeField]
    private int cluster = 0;

    public int Cluster => cluster;

    public Wolf Spawn()
    {
        Wolf wolf = Instantiate(wolfPrefab, transform.position, Quaternion.identity);
        RoomManager.Instance.TryAddToCache(wolf);
        return wolf;
    }
}
