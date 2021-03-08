using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public int id = 0;
    [SerializeField] public SpawnerDictionary prefabLookup = new SpawnerDictionary(defaultValue: null);

    public int Id => id;

    public Actor Spawn(ActorType actorType)
    {
        if (prefabLookup.ContainsKey(actorType)) return Instantiate(prefabLookup[actorType], transform.position, transform.rotation);

        Debug.LogError($"No prefab found for actor type: {actorType.ToString()}.");
        return null;
    }
}
