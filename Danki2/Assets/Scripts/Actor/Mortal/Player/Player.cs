using UnityEngine;

public enum CastingStatus
{
    ChannelingLeft,
    ChannelingRight,
    Cooldown,
    Ready
}

public enum CastingCommand
{
    ContinueChannel,
    CancelChannel,
    CastLeft,
    CastRight,
    None
}

public enum ActionControlState
{
    Left,
    Right,
    Both,
    None
}

public class Player : Mortal
{
    [HideInInspector]
    public float abilityCooldown = 1f;
    [HideInInspector]
    public float totalDashCooldown = 1f;
    [HideInInspector]
    public float dashDuration = 0.2f;
    [HideInInspector]
    public float dashSpeedMultiplier = 3f;

    private float _remainingDashCooldown = 0f;
    private CastingStatus _castingStatus = CastingStatus.Ready;
    private ActionControlState _previousActionControlState = ActionControlState.None;
    private AbilityTree _abilityTree;
    private ChannelService _channelService;
    private float _remainingCooldown = 0f;

    protected override void Awake()
    {
        base.Awake();

        _channelService = new ChannelService();

        _abilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Fireball,
                AbilityTreeFactory.CreateNode(AbilityReference.ShieldBash),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.ShieldBash,
                AbilityTreeFactory.CreateNode(AbilityReference.Slash),
                AbilityTreeFactory.CreateNode(AbilityReference.ShieldBash)
            )
        );
    }

    protected override void Update()
    {
        base.Update();

        TickDashCooldown();
        TickAbilityCooldown();

        _channelService.Update();
    }

    private void TickDashCooldown()
    {
        _remainingDashCooldown = Mathf.Max(0f, _remainingDashCooldown - Time.deltaTime);
    }

    private void TickAbilityCooldown()
    {
        _remainingCooldown = Mathf.Max(0f, _remainingCooldown - Time.deltaTime);
        if (_remainingCooldown == 0f && _castingStatus == CastingStatus.Cooldown)
        {
            _castingStatus = CastingStatus.Ready;
        }
    }

    public void Dash(Vector3 direction)
    {
        if (_remainingDashCooldown <= 0)
        {
            LockMovement(dashDuration, GetStat(Stat.Speed) * dashSpeedMultiplier, direction);
            _remainingDashCooldown = totalDashCooldown;
        }
    }

    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }
}
