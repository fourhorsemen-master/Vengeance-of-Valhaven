using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System;

public abstract class Ability
{
    private readonly string fmodVocalisationEvent;
    private readonly string fmodStartEvent;
    private readonly string fmodEndEvent;

    private readonly AbilityAnimationType animationType;

    private readonly List<EventInstance> vocalisationEventInstances = new List<EventInstance>();
    private readonly List<EventInstance> startEventInstances = new List<EventInstance>();
    private readonly List<EventInstance> endEventInstances = new List<EventInstance>();

    protected Actor Owner { get; }
    
    private AbilityData AbilityData { get; }

    private string[] ActiveBonuses { get; }

    protected Ability( AbilityConstructionArgs arguments )
    {
        Owner = arguments.Owner;
        AbilityData = arguments.AbilityDataObject;
        fmodVocalisationEvent = arguments.FmodVocalisationEvent;
        fmodStartEvent = arguments.FmodStartEvent;
        fmodEndEvent = arguments.FmodEndEvent;
        ActiveBonuses = arguments.ActiveBonuses;
        animationType = arguments.Animation;
    }

    protected void DealPrimaryDamage(Actor target, int linearDamageModifier = 0, int multiplicativeDamageModifier = 1)
    {
        int damage = GetModifiedValue(AbilityData.PrimaryDamage, linearDamageModifier, multiplicativeDamageModifier);
        target.HealthManager.ReceiveDamage(damage, Owner);
    }

    protected void DealSecondaryDamage(Actor target, int linearDamageModifier = 0, int multiplicativeDamageModifier = 1)
    {
        int damage = GetModifiedValue(AbilityData.SecondaryDamage, linearDamageModifier, multiplicativeDamageModifier);
        target.HealthManager.ReceiveDamage(damage, Owner);
    }

    protected void Heal(int linearHealModifier = 0, int multiplicativeHealModifier = 1)
    {
        Owner.HealthManager.ReceiveHeal(GetModifiedValue(AbilityData.Heal, linearHealModifier, multiplicativeHealModifier));
    }

    protected bool HasBonus(string bonus)
    {
        return ActiveBonuses.Contains(bonus);
    }

    protected Quaternion GetMeleeCastRotation(Vector3 castDirection)
    {
        return AbilityUtils.GetMeleeCastRotation(castDirection);
    }

    protected void TemplateCollision(
        CollisionTemplateShape shape,
        float scale,
        Vector3 position,
        Quaternion rotation,
        Action<Actor> offensiveAction,
        CollisionSoundLevel? soundLevel = null
    )
    {
        AbilityUtils.TemplateCollision(Owner, shape, scale, position, rotation, offensiveAction, soundLevel);
    }

    protected void TemplateCollision(
        CollisionTemplateShape shape,
        Vector3 scale,
        Vector3 position,
        Quaternion rotation,
        Action<Actor> offensiveAction,
        CollisionSoundLevel? soundLevel = null
    )
    {
        AbilityUtils.TemplateCollision(Owner, shape, scale, position, rotation, offensiveAction, soundLevel);
    }

    protected void PlayVocalisationEvent(Vector3? position = null)
    {
        if (string.IsNullOrEmpty(fmodVocalisationEvent)) return;
        if (!position.HasValue) position = Owner.AbilitySource;

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(fmodVocalisationEvent, position.Value);
        vocalisationEventInstances.Add(eventInstance);
        eventInstance.start();
        eventInstance.release();
    }

    protected void StopVocalisationEvents() => vocalisationEventInstances.ForEach(e => e.stop(STOP_MODE.IMMEDIATE));

    protected void PlayStartEvent(Vector3? position = null)
    {
        if (string.IsNullOrEmpty(fmodStartEvent)) return;
        if (!position.HasValue) position = Owner.AbilitySource;

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(fmodStartEvent, position.Value);
        startEventInstances.Add(eventInstance);
        eventInstance.start();
        eventInstance.release();
    }

    protected void StopStartEvents() => startEventInstances.ForEach(e => e.stop(STOP_MODE.IMMEDIATE));

    protected void PlayEndEvent(Vector3? position = null)
    {
        if (string.IsNullOrEmpty(fmodEndEvent)) return;
        if (!position.HasValue) position = Owner.AbilitySource;

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(fmodEndEvent, position.Value);
        endEventInstances.Add(eventInstance);
        eventInstance.start();
        eventInstance.release();
    }

    protected void StopEndEvents() => endEventInstances.ForEach(e => e.stop(STOP_MODE.IMMEDIATE));

    private int GetModifiedValue(int baseValue, int linearModifier, int multiplicativeModifier) =>
        (baseValue + linearModifier) * multiplicativeModifier;
}
