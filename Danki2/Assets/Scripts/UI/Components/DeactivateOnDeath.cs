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
        
        Subject<DeathData> deathSubject = usePlayer
            ? ActorCache.Instance.Player.DeathSubject
            : actor.DeathSubject;

        deathSubject.Subscribe(_ => Deactivate());
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
