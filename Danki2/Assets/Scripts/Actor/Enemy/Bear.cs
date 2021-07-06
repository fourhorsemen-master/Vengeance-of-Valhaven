using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bear : Enemy
{
    [Header("FMOD Events"), EventRef, SerializeField]
    private string roarEvent = null;

    [Header("Swipe")]
    [SerializeField] private int swipeDamage = 0;
    [SerializeField] private float swipeDashDuration = 0;
    [SerializeField] private float swipeDashSpeedMultiplier = 0;
    [SerializeField] private float swipePauseDuration = 0;
    [SerializeField] private float swipeDamageRange = 0;

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

    [Header("Maul")]
    [SerializeField] private int maulDamage = 0;
    [SerializeField] private int maulBiteCount = 0;
    [SerializeField] private float maulBiteInterval = 0;
    [SerializeField] private float maulBiteRange = 0;
    [SerializeField] private float maulSlowDuration = 0;
    
    [Header("Cleave")]
    [SerializeField] private int cleaveDamage = 0;
    [SerializeField] private float cleaveRange = 0;
    [SerializeField] private float cleavePauseDuration = 0;
    [SerializeField] private float cleaveKnockBackDuration = 0;
    [SerializeField] private float cleaveKnockBackSpeed = 0;
    
    private Actor chargeTarget = null;
    private Vector3 chargeDirection;
    private bool charging = false;
    
    public override ActorType Type => ActorType.Bear;

    public Subject ChargeSubject { get; } = new Subject();
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
            ChargeSubject.Next();
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
        Vector3 floorTargetPosition = transform.position + transform.forward;
        MovementManager.LookAt(floorTargetPosition);
        MaulObject maulObject = MaulObject.Create(AbilitySource);
        this.ActOnInterval(maulBiteInterval, index => HandleMaulBite(index, maulObject), 0, maulBiteCount);
    }

    private void HandleMaulBite(int index, MaulObject maulObject)
    {
        Vector3 forward = transform.forward;
        Vector3 horizontalDirection = Vector3.Cross(forward, Vector3.up).normalized;
        int directionMultiplier = index % 2 == 1 ? 1 : -1;
        Vector3 randomisedCastDirection = forward.normalized + 0.25f * directionMultiplier * horizontalDirection;

        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(randomisedCastDirection);

        maulObject.Bite(castRotation);

        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge45,
            maulBiteRange,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(maulDamage, this);
                actor.EffectManager.AddActiveEffect(ActiveEffect.Slow, maulSlowDuration);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        MovementManager.Pause(maulBiteInterval);
    }

    public void Cleave()
    {
        // InstantCastService.TryCast(
        //     AbilityReference.Cleave,
        //     GetMeleeTargetPosition(transform.position),
        //     GetMeleeTargetPosition(Centre)
        // );
        Vector3 forward = transform.forward;
        Vector3 castDirection = forward;
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        CleaveObject.Create(AbilitySource, castRotation);

        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge180,
            cleaveRange,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(cleaveDamage, this);
                MaulKnockBack(actor);
                CustomCamera.Instance.AddShake(ShakeIntensity.High);
            }
        );

        MovementManager.LookAt(transform.position + forward);
        MovementManager.Pause(cleavePauseDuration);

        CleaveSubject.Next();
    }
    
    private void MaulKnockBack(Actor actor)
    {
        Vector3 knockBackDirection = actor.transform.position - transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.TryLockMovement(
            MovementLockType.Knockback,
            cleaveKnockBackDuration,
            cleaveKnockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
        );
    }

    private void Roar()
    {
        EventInstance fmodEvent = FmodUtils.CreatePositionedInstance(roarEvent, transform.position);
        fmodEvent.start();
        fmodEvent.release();
    }
}
