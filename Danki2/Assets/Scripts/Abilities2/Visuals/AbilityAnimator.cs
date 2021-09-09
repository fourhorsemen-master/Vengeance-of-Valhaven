using System;
using UnityEngine;

[Serializable]
public class AbilityTypeToAnimationTypeDictionary : SerializableEnumDictionary<AbilityType2, AbilityAnimationType>
{
    public AbilityTypeToAnimationTypeDictionary(AbilityAnimationType defaultValue) : base(defaultValue) {}
    public AbilityTypeToAnimationTypeDictionary(Func<AbilityAnimationType> defaultValueProvider) : base(defaultValueProvider) {}
}

public class AbilityAnimator : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField, HideInInspector]
    public AbilityTypeToAnimationTypeDictionary abilityTypeToAnimationTypeDictionary =
        new AbilityTypeToAnimationTypeDictionary(AbilityAnimationType.None);

    private void Start()
    {
        player.AbilityService.AbilityCastSubject.Subscribe(HandleAnimation);
    }

    private void HandleAnimation(AbilityCastInformation abilityCastInformation)
    {
        AbilityType2 type = AbilityLookup2.Instance.GetAbilityType(abilityCastInformation.AbilityId);
        AbilityAnimationType abilityAnimationType = abilityTypeToAnimationTypeDictionary[type];
        string animationState = AnimationStringLookup.LookupTable[abilityAnimationType];
        player.AnimController.Play(animationState);
    }
}
