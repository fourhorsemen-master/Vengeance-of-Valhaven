using UnityEngine;

public class Ent : Enemy
{
    [Header("Spine")]
    [SerializeField] private int spineMaxAngle = 0;
    [SerializeField] private int spineDamage = 0;
    [SerializeField] private float spineSlowDuration = 0;

    [Header("Swipe")]
    [SerializeField] private float swipeMovementDuration = 0;
    [SerializeField] private float swipePauseDuration = 0;
    [SerializeField] private float swipeRange = 0;
    [SerializeField] private int swipeDamage = 0;
    [SerializeField] private int swipeMovementSpeedMultiplier = 0;

    public override ActorType Type => ActorType.Ent;

    public void Spine(Actor target)
    {
        MovementManager.LookAt(target.Centre);
        MovementManager.SetRotationTarget(target.transform, null);

        Quaternion rotation = Quaternion.LookRotation(target.Centre - Centre);
        rotation *= Quaternion.Euler(0f, Random.Range(-spineMaxAngle, spineMaxAngle), 0f);
        SpineObject.Fire(this, OnSpineCollision, AbilitySource, rotation);
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
