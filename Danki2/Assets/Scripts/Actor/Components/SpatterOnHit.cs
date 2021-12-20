using UnityEngine;
using UnityEngine.VFX;

public class SpatterOnHit : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private VisualEffect spatterEffect = null;

    private void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(spatterEffect.Play);
    }
}
