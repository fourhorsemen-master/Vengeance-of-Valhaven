using UnityEngine;

public class DeactivateOnDeath : MonoBehaviour
{
    public bool usePlayer = false;
    public Actor actor = null;

    private void Start()
    {
        if (!usePlayer && actor == null)
        {
            Debug.LogError($"No actor found and not set to use player on {GetType()}");
            return;
        }
        
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
