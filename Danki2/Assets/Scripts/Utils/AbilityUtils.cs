using System;
using UnityEngine;

public static class AbilityUtils
{
    // Max angle and min vertical angles you can target with melee attacks
    private const float MaxMeleeVerticalAngle = 30f;
    private const float MinMeleeVerticalAngle = -30f;

    private const float DefaultCollisionTemplateHeight = 1.5f;

    public static Quaternion GetMeleeCastRotation(Vector3 castDirection)
    {
        Quaternion castRotation = Quaternion.LookRotation(castDirection);
        float castAngleX = castRotation.eulerAngles.x;

        if (castAngleX > 180f) castAngleX -= 360f;

        float newAngleX = Mathf.Clamp(castAngleX, MinMeleeVerticalAngle, MaxMeleeVerticalAngle);

        return Quaternion.Euler(newAngleX, castRotation.eulerAngles.y, castRotation.eulerAngles.z);
    }
    
    public static void TemplateCollision(
        Actor owner,
        CollisionTemplateShape shape,
        float scale,
        Vector3 position,
        Quaternion rotation,
        CollisionSoundLevel? soundLevel = null,
        Action<Player> playerCallback = null,
        Action<Enemy> enemyCallback = null
    )
    {
        TemplateCollision(owner, shape, new Vector3(scale, DefaultCollisionTemplateHeight, scale), position, rotation, soundLevel, playerCallback, enemyCallback);
    }

    public static void TemplateCollision(
        Actor owner,
        CollisionTemplateShape shape,
        Vector3 scale,
        Vector3 position,
        Quaternion rotation,
        CollisionSoundLevel? soundLevel = null,
        Action<Player> playerCallback = null,
        Action<Enemy> enemyCallback = null
    )
    {
        CollisionTemplate template = CollisionTemplateManager.Instance.Create(owner, shape, scale, position, rotation);

        template.GetCollidingActors().ForEach(actor =>
        {
            if (actor.CompareTag(Tag.Player)) playerCallback?.Invoke(actor as Player);
            if (actor.CompareTag(Tag.Enemy)) enemyCallback?.Invoke(actor as Enemy);
        });

        if (soundLevel.HasValue) template.PlayCollisionSound(soundLevel.Value);
    }
}
