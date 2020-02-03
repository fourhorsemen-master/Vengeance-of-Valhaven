using UnityEngine;

public class Pounce : Channel
{
    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 0.8f;
    private float _initialRootDuration = 0.3f;
    private float _initialRootTimer = 0f;
    private bool _pounceStarted = false;
    private float _finalRootDuration = 0.3f;

    public static float Range => 10f;
    public static readonly float PounceSpeedMultiplier = 3f;

    public override void Start()
    {
        Debug.Log("Pounce start");
        Context.Owner.Root(
            Duration,
            (Context.TargetPosition - Context.Owner.transform.position)
        );

        _initialRootTimer = _initialRootDuration;
    }

    public override void Continue()
    {
        Debug.Log("Pounce continuing");
        _initialRootTimer -= Time.deltaTime;

        if (_initialRootTimer >= 0f || _pounceStarted)
        {
            return;
        }

        _pounceStarted = true;

        Vector3 targetPosition = Context.TargetPosition;
        targetPosition.y = Context.Owner.transform.position.y;

        Context.Owner.LockMovement(
            (Duration - _initialRootDuration),
            Context.Owner.GetStat(Stat.Speed) * PounceSpeedMultiplier,
            (targetPosition - Context.Owner.transform.position),
            @override: true
        );
    }

    public override void Cancel()
    {
        Debug.Log("Pounce cancelled");
    }

    public override void End()
    {
        Debug.Log("Pounce end");

        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        float damage = owner.GetStat(Stat.Strength);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(target - position)
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
            }
        });

        owner.Root(_finalRootDuration, owner.transform.forward, true);
    }
}
