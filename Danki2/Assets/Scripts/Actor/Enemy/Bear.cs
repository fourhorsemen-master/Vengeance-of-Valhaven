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

    [Header("Charge")]
    [SerializeField] private int chargeDamage = 0;
    [SerializeField] private float chargeSpeed = 0;
    [SerializeField] private float chargeEffectInterval = 0;
    [SerializeField] private int chargeEffectRepetitions = 0;
    [SerializeField] private float chargeRotationRate = 0;
    [SerializeField] private float chargeDamageRange = 0;
    [SerializeField] private float chargePauseDuration = 0;
    [SerializeField] private float chargeKnockBackDuration = 0;
    [SerializeField] private float chargeKnockBackSpeed = 0;

    private Actor chargeTarget = null;
    private Vector3 chargeDirection;
    private bool charging = false;
    
    public override ActorType Type => ActorType.Bear;

    public Subject CleaveSubject { get; } = new Subject();

    protected override void Start()
    {
        base.Start();

        HealthManager.ModifiedDamageSubject.Subscribe(_ => Roar());
    }

    protected override void Update()
    {
        base.Update();
        
        if (charging) ContinueCharge();
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

    public void Charge(Actor target)
    {
        chargeTarget = target;
        chargeDirection = target.transform.position - transform.position;
        charging = true;
        this.ActOnInterval(chargeEffectInterval, ChargeEffect, chargeEffectInterval, chargeEffectRepetitions);
    }

    private void ContinueCharge()
    {
        Vector3 desiredDirection = chargeTarget.transform.position - transform.position;
        chargeDirection = Vector3.RotateTowards(
            chargeDirection,
            desiredDirection,
            chargeRotationRate * Time.deltaTime,
            Mathf.Infinity
        );
        MovementManager.Move(chargeDirection, chargeSpeed);
    }

    private void ChargeEffect(int index)
    {
        if (index == chargeEffectRepetitions - 1)
        {
            MovementManager.Pause(chargePauseDuration);
            charging = false;
        }
        
        Quaternion castRotation = Quaternion.LookRotation(transform.forward);
        
        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge90,
            chargeDamageRange,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                ChargeKnockBack(actor);
                actor.HealthManager.ReceiveDamage(chargeDamage, this);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        SwipeObject.Create(AbilitySource, castRotation);
    }

    private void ChargeKnockBack(Actor actor)
    {
        Vector3 knockBackDirection = actor.transform.position - transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.TryLockMovement(
            MovementLockType.Knockback,
            chargeKnockBackDuration,
            chargeKnockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
        );
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
