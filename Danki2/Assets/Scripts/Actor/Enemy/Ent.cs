using UnityEngine;

public class Ent : Enemy
{
    [Header("Spine")]
    [SerializeField] private int spineMaxAngle;
    [SerializeField] private float spineSpeed;
    [SerializeField] private int spineDamage;
    [SerializeField] private float spineSlowDuration;

    [Header("Swipe")]
    [SerializeField] private float swipeMovementDuration;
    [SerializeField] private float swipePauseDuration;
    [SerializeField] private float swipeRange;
    [SerializeField] private int swipeDamage;
    [SerializeField] private int swipeMovementSpeedMultiplier;

    public override ActorType Type => ActorType.Ent;

    public void Spine(Actor target)
    {
        MovementManager.LookAt(target.Centre);
        MovementManager.Watch(target.transform);

        Quaternion rotation = Quaternion.LookRotation(target.Centre - Centre);
        rotation *= Quaternion.Euler(0f, Random.Range(-spineMaxAngle, spineMaxAngle), 0f);
        SpineObject.Fire(this, OnSpineCollision, spineSpeed, AbilitySource, rotation);
    }

    public void Swipe()
    {
        Vector3 castDirection = transform.forward;
        float lungeSpeed = Speed * swipeMovementSpeedMultiplier;

        MovementManager.LockMovement(swipeMovementDuration, lungeSpeed, castDirection, castDirection);

        StartTrail(swipeMovementDuration + swipePauseDuration);

        this.WaitAndAct(swipeMovementDuration, () => SwipeDamageOnLand(castDirection));
    }

    private void OnSpineCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor) && actor.Opposes(this))
        {
            actor.HealthManager.ReceiveDamage(spineDamage, this);
            actor.EffectManager.AddActiveEffect(ActiveEffect.Slow, spineSlowDuration);
            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        }
    }

    private void SwipeDamageOnLand(Vector3 castDirection)
    {
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(castDirection);

        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge90,
            swipeRange,
            CollisionTemplateSource,
            castRotation,
            CollisionSoundLevel.Low,
            player =>
            {
                player.HealthManager.ReceiveDamage(swipeDamage, this);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        WraithSwipeObject.Create(AbilitySource, castRotation);

        MovementManager.Pause(swipePauseDuration);
    }
}
