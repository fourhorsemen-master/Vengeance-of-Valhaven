using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    private const float FlashIntensity = 0.3f;
    private const float FlashDuration = 0.1f;

    void Start()
    {
        actor.HealthManager.ModifiedDamageSubject.Subscribe(_ => Flash());
    }

    public void Flash()
    {
        actor.HighlightManager.AddTemporaryHighlight(FlashIntensity, FlashDuration);
    }
}
