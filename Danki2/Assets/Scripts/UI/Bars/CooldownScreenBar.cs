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
        this.player = RoomManager.Instance.Player;
    }

    private void Update()
    {
        SetWidth(this.player.RemainingAbilityCooldown / this.player.abilityCooldown);
    }
}
