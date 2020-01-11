public class CooldownBar : Bar
{
    Player _player;

    void Awake()
    {
        SetWidth(0f);
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        SetWidth(_player.RemainingAbilityCooldown / _player.abilityCooldown);
    }
}
