using System.Collections.Generic;
using UnityEngine;

public class Diegetic : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private List<GameObject> deactivateOnDeath = null;

    public Actor Actor => actor;

    private void Start()
    {
        Actor.DeathSubject.Subscribe(() =>
            deactivateOnDeath.ForEach(o => o.gameObject.SetActive(false))
        );
    }
}
