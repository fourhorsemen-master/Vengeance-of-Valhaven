using UnityEngine;

public class AbilityDashManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float dashDuration = 0;

    [SerializeField]
    private float dashSpeed = 0;

    private void Start()
    {
        player.AbilityAnimationListener.DashSubject
            .Subscribe(HandleDash);
    }

    private void HandleDash()
    {
        player.MovementManager.TryLockMovement(
            MovementLockType.AbilityDash,
            dashDuration,
            dashSpeed,
            player.transform.forward,
            player.transform.forward
        );
    }
}
