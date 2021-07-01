using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bear : Enemy
{
    [Header("FMOD Events"), EventRef, SerializeField]
    private string roarEvent = null;

    [Header("Swipe")]
    [SerializeField] private int swipeDamage = 0;
    [SerializeField] private float swipeDashDuration = 0f;
    [SerializeField] private float swipeDashSpeedMultiplier = 0f;
    [SerializeField] private float swipePauseDuration = 0f;
    [SerializeField] private float swipeDamageRange = 0f;

    public override ActorType Type => ActorType.Bear;

    public Subject CleaveSubject { get; } = new Subject();

    protected override void Start()
    {
        base.Start();

        HealthManager.ModifiedDamageSubject.Subscribe(_ => Roar());
    }

    public void Swipe()
    {
        Vector3 forward = transform.forward;
        MovementManager.LookAt(transform.position + forward);

        MovementManager.TryLockMovement(
            MovementLockType.Dash,
            swipeDashDuration,
            StatsManager.Get(Stat.Speed) * swipeDashSpeedMultiplier,
            forward,
            forward
        );
        
        InterruptibleAction(swipeDashDuration, InterruptionType.Hard, HandleSwipeLand);
    }
    
    private void HandleSwipeLand()
    {
        Quaternion castRotation = Quaternion.LookRotation(transform.forward);
        
        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge90,
            swipeDamageRange,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(swipeDamage, this);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        SwipeObject.Create(AbilitySource, castRotation);

        MovementManager.Pause(swipePauseDuration);
    }

    public void Charge()
    {
        ChannelService.TryStartChannel(AbilityReference.BearCharge);
    }

    public void Maul()
    {
        InstantCastService.TryCast(
            AbilityReference.Maul,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Cleave,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );

        CleaveSubject.Next();
    }

    private void Roar()
    {
        EventInstance fmodEvent = FmodUtils.CreatePositionedInstance(roarEvent, transform.position);
        fmodEvent.start();
        fmodEvent.release();
    }
}
