using UnityEngine;

public class CooldownScreenBar : ScreenBar
{
    private Player player;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Start()
    {
        player = RoomManager.Instance.Player;
    }

    private void Update()
    {
        SetWidth(player.AbilityManager.RemainingAbilityCooldown / player.abilityCooldown);
    }
}
