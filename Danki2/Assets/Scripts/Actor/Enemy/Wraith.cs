using UnityEngine;
using UnityEngine.VFX;

public class Wraith : Enemy
{
    [Header("Blink")]
    [SerializeField] private VisualEffect blinkVisualEffect = null;
    
    [Header("Spine")]
    [SerializeField] private int spineDamage = 0;
    [SerializeField] private float spineSpeed = 0;
    [SerializeField] private float spineSlowDuration = 0;
    [SerializeField] private int spineCount = 0;
    [SerializeField] private float spineMaxAngle = 0;
    [SerializeField] private float spineInterval = 0;
    [SerializeField] private float spinePauseDuration = 0;
    
    [Header("Guided Orb")]
    [SerializeField] private int guidedOrbDamage = 0;
    [SerializeField] private float guidedOrbMaxDuration = 0;
    [SerializeField] private float guidedOrbSpeed = 0;
    [SerializeField] private float guidedOrbRotationSpeed = 0;
    [SerializeField] private float guidedOrbRange = 0;
    
    public Subject SwipeSubject { get; } = new Subject();

    public override ActorType Type => ActorType.Wraith;

    public void Spine(Actor target)
    {
        MovementManager.LookAt(target.Centre);
        MovementManager.Pause(spinePauseDuration);

        for (int i = 0; i < spineCount; i++)
        {
            this.WaitAndAct(
                i * spineInterval,
                () =>
                {
                    Quaternion rotation = Quaternion.LookRotation(target.Centre - Centre);
                    rotation *= Quaternion.Euler(0f, Random.Range(-spineMaxAngle, spineMaxAngle), 0f);
                    SpineObject.Fire(this, OnSpineCollision, spineSpeed, AbilitySource, rotation);
                }
            );
        }
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

    public void GuidedOrb(Actor target)
    {
        GuidedOrbObject.Fire(
            guidedOrbMaxDuration,
            guidedOrbSpeed,
            guidedOrbRotationSpeed,
            target.CentreTransform,
            AbilitySource,
            HandleGuidedOrbExplosion,
            DeathSubject
        );
    }

    private void HandleGuidedOrbExplosion(Vector3 position)
    {
        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Sphere,
            guidedOrbRange,
            position,
            Quaternion.identity,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(guidedOrbDamage, this);
            }
        );

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.WraithSwipe,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
        SwipeSubject.Next();
    }

    public void TelegraphBlink()
    {
        blinkVisualEffect.Reinit();
        blinkVisualEffect.Play();
    }
    
    public void Blink(Vector3 target)
    {
        InstantCastService.TryCast(
            AbilityReference.Blink,
            target,
            target + Height * Vector3.up
        );
    }
}
