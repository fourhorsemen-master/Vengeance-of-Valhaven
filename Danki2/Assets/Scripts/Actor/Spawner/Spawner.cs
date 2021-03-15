using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public SpawnerDictionary prefabLookup = new SpawnerDictionary(defaultValue: null);

    public Actor Spawn(ActorType actorType)
    {
        if (prefabLookup.ContainsKey(actorType)) return Instantiate(prefabLookup[actorType], transform.position, transform.rotation);

        Debug.LogError($"No prefab found for actor type: {actorType.ToString()}.");
        return null;
    }
}
