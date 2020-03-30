using UnityEngine;

public class CooldownScreenBar : ScreenBar
{
    [SerializeField]
    private Player _player = null;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Update()
    {
        SetWidth(_player.RemainingAbilityCooldown / _player.abilityCooldown);
    }
}
