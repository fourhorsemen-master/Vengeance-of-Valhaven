using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    void Start()
    {
        actor.HealthManager.ModifiedDamageSubject.Subscribe(_ => Flash());
    }

    public void Flash()
    {
        actor.EmissiveManager.Flash();
    }
}
