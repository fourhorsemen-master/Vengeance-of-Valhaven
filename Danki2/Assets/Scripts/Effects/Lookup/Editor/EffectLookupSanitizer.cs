public class EffectLookupSanitizer
{
    public void SanitizeActiveEffects(SerializableActiveEffectDictionary serializableActiveEffectDictionary)
    {
        EnumUtils.ForEach((ActiveEffect effect) =>
        {
            if (serializableActiveEffectDictionary[effect] == null)
            {
                serializableActiveEffectDictionary[effect] = new SerializableActiveEffectData();
            }
        });
    }
    
    public void SanitizePassiveEffects(SerializablePassiveEffectDictionary serializablePassiveEffectDictionary)
    {
        EnumUtils.ForEach((PassiveEffect effect) =>
        {
            if (serializablePassiveEffectDictionary[effect] == null)
            {
                serializablePassiveEffectDictionary[effect] = new SerializablePassiveEffectData();
            }
        });
    }
    
    public void SanitizeStackingEffects(SerializableStackingEffectDictionary serializableStackingEffectDictionary)
    {
        EnumUtils.ForEach((StackingEffect effect) =>
        {
            if (serializableStackingEffectDictionary[effect] == null)
            {
                serializableStackingEffectDictionary[effect] = new SerializableStackingEffectData();
            }
        });
    }
}
