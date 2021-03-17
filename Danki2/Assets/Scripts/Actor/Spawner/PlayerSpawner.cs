using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private Spawner spawner = null;

    public int Id { get => id; set => id = value; }
    public Spawner Spawner { get => spawner; set => spawner = value; }

    public void Spawn()
    {
        spawner.Spawn(ActorType.Player);
    }
}
