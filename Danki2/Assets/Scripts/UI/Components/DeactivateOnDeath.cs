using UnityEngine;

public class DeactivateOnDeath : MonoBehaviour
{
    public bool usePlayer = false;
    public Actor actor = null;

    private void Start()
    {
        Subject deathSubject = usePlayer
            ? RoomManager.Instance.Player.DeathSubject
            : actor.DeathSubject;

        deathSubject.Subscribe(Deactivate);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
