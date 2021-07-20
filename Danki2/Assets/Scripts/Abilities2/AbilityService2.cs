using UnityEngine;

public class AbilityService2
{
    private const float AbilityRange = 3;
    private const float AbilityPauseDuration = 0.3f;
    
    private readonly Player player;
    
    public Subject AbilityCastSubject { get; } = new Subject();

    public AbilityService2(Player player)
    {
        this.player = player;
    }

    public void Cast(Ability2 ability, Vector3 targetPosition)
    {
        player.MovementManager.LookAt(targetPosition);
        player.MovementManager.Pause(AbilityPauseDuration);
        
        Vector3 castDirection = targetPosition - player.transform.position;
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(castDirection);
        
        bool hasDealtDamage = false;
        
        AbilityUtils.TemplateCollision(
            player,
            CollisionTemplateShape.Wedge90,
            AbilityRange,
            player.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                hasDealtDamage = true;
                HandleCollision(ability, actor);
            },
            ability.CollisionSoundLevel
        );
        
        HandleCameraShake(hasDealtDamage);
        
        AbilityCastSubject.Next();
    }

    private void HandleCollision(Ability2 ability, Actor actor)
    {
        actor.HealthManager.ReceiveDamage(ability.Damage, player);
    }

    private void HandleCameraShake(bool hasDealtDamage)
    {
        CustomCamera.Instance.AddShake(hasDealtDamage ? ShakeIntensity.Medium : ShakeIntensity.Low);
    }
}
