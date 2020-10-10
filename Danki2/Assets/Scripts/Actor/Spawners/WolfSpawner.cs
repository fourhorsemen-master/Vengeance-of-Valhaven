using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    [SerializeField]
    private Wolf wolfPrefab = null;

    [SerializeField]
    private Wolf alphaWolfPrefab = null;

    [SerializeField]
    private int cluster = 0;

    public int Cluster => cluster;

    public Wolf SpawnWolf()
    {
        Wolf wolf = Instantiate(wolfPrefab, transform.position, Quaternion.identity);
        RoomManager.Instance.TryAddToCache(wolf);
        return wolf;
    }

    public Wolf SpawnAlphaWolf()
    {
        Wolf alphaWolf = Instantiate(alphaWolfPrefab, transform.position, Quaternion.identity);
        RoomManager.Instance.TryAddToCache(alphaWolf);
        return alphaWolf;
    }
}
