using UnityEngine;

public class CooldownScreenBar : ScreenBar
{
    [SerializeField]
    private Player _player = null;

    void Awake()
    {
        SetWidth(0f);
    }

    void Update()
    {
        SetWidth(_player.RemainingAbilityCooldown / _player.abilityCooldown);
    }
}
