using System.Collections.Generic;
using UnityEngine;

public enum AbilityEmpowerment
{
    DoubleDamage,
    Rupture,
    Explode
}

public class Ability2
{
    public int Damage { get; }
    public List<AbilityEmpowerment> Empowerments { get; }
    
    public CollisionSoundLevel CollisionSoundLevel { get; }
    public Sprite Icon { get; }

    public Ability2(
        int damage,
        List<AbilityEmpowerment> empowerments,
        CollisionSoundLevel collisionSoundLevel,
        Sprite icon
    )
    {
        Damage = damage;
        Empowerments = empowerments;
        CollisionSoundLevel = collisionSoundLevel;
        Icon = icon;
    }
}
