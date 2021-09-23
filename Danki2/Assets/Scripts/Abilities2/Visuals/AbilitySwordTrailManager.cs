using UnityEngine;

public class AbilitySwordTrailManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TrailRenderer swordTrailRenderer;

    private void Start()
    {
        player.AbilityAnimationListener.SwingSubject.Subscribe(
            () => swordTrailRenderer.emitting = true
        );

        player.AbilityAnimationListener.FinishSubject.Subscribe(
            () => swordTrailRenderer.emitting = false
        );

        player.ComboManager.SubscribeToStateEntry(
            ComboState.Interrupted,
            () => swordTrailRenderer.emitting = false
        );
    }
}
