using System;
using UnityEngine;

public class EffectLookupValidator
{
    public bool HasErrors { get; private set; } = false;
    
    public void ValidateActiveEffects(SerializableActiveEffectDictionary serializableActiveEffectDictionary)
    {
        ValidateDictionaryEntries(serializableActiveEffectDictionary);
        EnumUtils.ForEach((ActiveEffect effect) =>
            ValidateBaseEffectData(effect, serializableActiveEffectDictionary[effect]));
    }
    
    public void ValidatePassiveEffects(SerializablePassiveEffectDictionary serializablePassiveEffectDictionary)
    {
        ValidateDictionaryEntries(serializablePassiveEffectDictionary);
        EnumUtils.ForEach((PassiveEffect effect) =>
            ValidateBaseEffectData(effect, serializablePassiveEffectDictionary[effect]));
    }
    
    public void ValidateStackingEffects(SerializableStackingEffectDictionary serializableStackingEffectDictionary)
    {
        ValidateDictionaryEntries(serializableStackingEffectDictionary);
        EnumUtils.ForEach((StackingEffect effect) =>
            ValidateStackingEffect(effect, serializableStackingEffectDictionary[effect]));
    }

    private void ValidateDictionaryEntries<TEnum, TEffectData>(
        SerializableEnumDictionary<TEnum, TEffectData> effectDataDictionary
    )
        where TEnum : Enum where TEffectData : SerializableEffectData
    {
        EnumUtils.ForEach((TEnum effect) =>
        {
            if (!effectDataDictionary.ContainsKey(effect) || effectDataDictionary[effect] == null)
            {
                LogError($"No effect data found for effect: {effect.ToString()}.");
            }
        });
    }

    private void ValidateStackingEffect(StackingEffect effect, SerializableStackingEffectData effectData)
    {
        ValidateBaseEffectData(effect, effectData);

        if (effectData.MaxStackSize <= 0)
        {
            LogError($"Maximum stack size is less than or equal to 0 for stacking effect: {effect.ToString()}.");
        }

        if (effectData.Duration <= 0)
        {
            LogError($"Duration is less than or equal to 0 for stacking effect: {effect.ToString()}.");
        }
    }

    private void ValidateBaseEffectData<T>(T effect, SerializableEffectData effectData) where T : Enum
    {
        if (string.IsNullOrWhiteSpace(effectData.DisplayName))
        {
            LogError($"No display name found for effect: {effect.ToString()}.");
        }

        if (effectData.Sprite == null)
        {
            LogError($"No sprite found for effect: {effect.ToString()}.");
        }
    }
    
    private void LogError(string message)
    {
        Debug.LogError(message);
        HasErrors = true;
    }
}
