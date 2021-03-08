using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Wolf wolfPrefab;
    [SerializeField] private Bear bearPrefab;

    public int Id => id;
    
    public Actor Spawn(ActorType actorType)
    {
        switch (actorType)
        {
            case ActorType.Wolf:
                return Instantiate(wolfPrefab, transform.position, transform.rotation);
            case ActorType.Bear:
                return Instantiate(bearPrefab, transform.position, transform.rotation);
            default:
                Debug.LogError($"Unable to spawn actor of type: {actorType.ToString()}");
                return null;
        }
    }
}
