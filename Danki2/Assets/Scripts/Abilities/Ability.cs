using System.Collections.Generic;
using FMODUnity;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System;

public class AbilityContructionArgs
{
    public AbilityContructionArgs(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] activeBonuses, AbilityAnimationType animationType)
	{
        this.owner = owner;
        this.abilityData = abilityData;
        this.fmodStartEvent = fmodStartEvent;
        this.fmodEndEvent = fmodEndEvent;
        this.activeBonuses = activeBonuses;
        this.animation = animationType;
	}

	public AbilityContructionArgs(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] activeBonuses, AbilityAnimationType animationType, float? channelDuration)
	{
		this.owner = owner;
		this.abilityData = abilityData;
		this.fmodStartEvent = fmodStartEvent;
		this.fmodEndEvent = fmodEndEvent;
		this.activeBonuses = activeBonuses;
		this.animation = animationType;
		this.channelDuration = channelDuration;
	}
	public Actor owner;
    public AbilityData abilityData;
    public string fmodStartEvent;
    public string fmodEndEvent;
    public string[] activeBonuses;
    public AbilityAnimationType animation;
    public float? channelDuration;
}

public abstract class Ability
{
    // Max angle and min vertical angles you can target with melee attacks
    public const float MaxMeleeVerticalAngle = 30f;
    public const float MinMeleeVerticalAngle = -30f;

    private readonly string fmodStartEvent;
    private readonly string fmodEndEvent;

    private readonly AbilityAnimationType animationType;

    private readonly List<EventInstance> startEventInstances = new List<EventInstance>();
    private readonly List<EventInstance> endEventInstances = new List<EventInstance>();
    
    public Subject<bool> SuccessFeedbackSubject { get; }

    protected Actor Owner { get; }
    
    private AbilityData AbilityData { get; }

    private string[] ActiveBonuses { get; }

    protected Ability( AbilityContructionArgs arguments )
    {
        Owner = arguments.owner;
        AbilityData = arguments.abilityData;
        this.fmodStartEvent = arguments.fmodStartEvent;
        this.fmodEndEvent = arguments.fmodEndEvent;
        ActiveBonuses = arguments.activeBonuses;
        animationType = arguments.animation;
        SuccessFeedbackSubject = new Subject<bool>();
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
        Quaternion castRotation = Quaternion.LookRotation(castDirection);
        float castAngleX = castRotation.eulerAngles.x;

        if (castAngleX > 180f) castAngleX -= 360f;

        float newAngleX = Mathf.Clamp(castAngleX, MinMeleeVerticalAngle, MaxMeleeVerticalAngle);

        return Quaternion.Euler(newAngleX, castRotation.eulerAngles.y, castRotation.eulerAngles.z);
    }

    protected void TemplateCollision(CollisionTemplate template, float scale, Vector3 position, Quaternion rotation, Action<Actor> offensiveAction)
    {
        TemplateCollision(template, scale * Vector3.one, position, rotation, offensiveAction);
    }

    protected void TemplateCollision(CollisionTemplate template, Vector3 scale, Vector3 position, Quaternion rotation, Action<Actor> offensiveAction)
    {
        // TODO: handle collision sounds and screenshake here?

        CollisionTemplateManager.Instance.GetCollidingActors(template, scale, position, rotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(offensiveAction);
    }

    protected void PlayStartEvent()
    {
        if (string.IsNullOrEmpty(fmodStartEvent)) return;

        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodStartEvent);
        startEventInstances.Add(eventInstance);
        eventInstance.start();
        eventInstance.release();
    }

    protected void StopStartEvents() => startEventInstances.ForEach(e => e.stop(STOP_MODE.IMMEDIATE));

    protected void PlayEndEvent()
    {
        if (string.IsNullOrEmpty(fmodEndEvent)) return;

        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEndEvent);
        endEventInstances.Add(eventInstance);
        eventInstance.start();
        eventInstance.release();
    }

    protected void StopEndEvents() => endEventInstances.ForEach(e => e.stop(STOP_MODE.IMMEDIATE));

    private int GetModifiedValue(int baseValue, int linearModifier, int multiplicativeModifier) =>
        (baseValue + linearModifier) * multiplicativeModifier;
}
