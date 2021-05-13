using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private Spawner spawner = null;

    public int Id { get => id; set => id = value; }
    public Spawner Spawner { get => spawner; set => spawner = value; }

    public void Spawn()
    {
        Pole entranceSide = SceneLookup.Instance.GetEntranceSide(PersistenceManager.Instance.SaveData.CurrentRoomNode.Scene, id);
        float yRotation = OrientationUtils.GetYRotation(OrientationUtils.GetReversedPole(entranceSide));

        spawner.Spawn(ActorType.Player).transform.Rotate(0, yRotation, 0);
    }
}
