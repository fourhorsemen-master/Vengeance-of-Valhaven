using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private Spawner spawner = null;

    public int Id { get => id; set => id = value; }
    public Spawner Spawner { get => spawner; set => spawner = value; }

    public Enemy Spawn(ActorType actorType)
    {
        if (actorType == ActorType.Player)
        {
            Debug.LogError("Tried to spawn player from enemy spawner");
            return null;
        }

        return (Enemy) spawner.Spawn(actorType);
    }
}
