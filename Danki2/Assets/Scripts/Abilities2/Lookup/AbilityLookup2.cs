using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLookup2 : Singleton<AbilityLookup2>
{
    [SerializeField] private AbilityData abilityData = null;

    public string GetDisplayName(SerializableGuid abilityId) => abilityData.GetDisplayName(abilityId);
    public AbilityType2 GetAbilityType(SerializableGuid abilityId) => abilityData.GetAbilityType(abilityId);
    public int GetDamage(SerializableGuid abilityId) => abilityData.GetDamage(abilityId);
    public List<Empowerment> GetEmpowerments(SerializableGuid abilityId) => abilityData.GetEmpowerments(abilityId);
    public Rarity GetRarity(SerializableGuid abilityId) => abilityData.GetRarity(abilityId);
    public CollisionSoundLevel GetCollisionSoundLevel(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetCollisionSoundLevel(GetAbilityType(abilityId));
    public AbilityVocalisationType GetAbilityVocalisationType(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetAbilityVocalisationType(GetAbilityType(abilityId));
    public AbilityFmodEvents GetAbilityFmodEvents(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetAbilityFmodEvents(GetAbilityType(abilityId));
    public Sprite GetIcon(SerializableGuid abilityId) => abilityData.GetIcon(abilityId);

    public bool TryGetAbilityId(string displayName, out SerializableGuid abilityId) => abilityData.TryGetAbilityId(displayName, out abilityId);

    public void ForEachAbilityId(Action<SerializableGuid> action) => abilityData.ForEachAbilityId(action);
}
