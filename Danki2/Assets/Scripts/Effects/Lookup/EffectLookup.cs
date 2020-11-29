using UnityEngine;

public class EffectLookup : Singleton<EffectLookup>
{
    public SerializableActiveEffectDictionary serializableActiveEffectDictionary =
        new SerializableActiveEffectDictionary(() => new SerializableActiveEffectData());
    public SerializablePassiveEffectDictionary serializablePassiveEffectDictionary =
        new SerializablePassiveEffectDictionary(() => new SerializablePassiveEffectData());
    public SerializableStackingEffectDictionary serializableStackingEffectDictionary =
        new SerializableStackingEffectDictionary(() => new SerializableStackingEffectData());

    protected override void Awake()
    {
        base.Awake();
        
        Validate();
    }

    public string GetDisplayName(ActiveEffect effect) => serializableActiveEffectDictionary[effect].DisplayName;
    public string GetDisplayName(PassiveEffect effect) => serializablePassiveEffectDictionary[effect].DisplayName;
    public string GetDisplayName(StackingEffect effect) => serializableStackingEffectDictionary[effect].DisplayName;

    public Sprite GetSprite(ActiveEffect effect) => serializableActiveEffectDictionary[effect].Sprite;
    public Sprite GetSprite(PassiveEffect effect) => serializablePassiveEffectDictionary[effect].Sprite;
    public Sprite GetSprite(StackingEffect effect) => serializableStackingEffectDictionary[effect].Sprite;

    public bool HasMaxStackSize(StackingEffect effect) => serializableStackingEffectDictionary[effect].HasMaxStackSize;
    public int GetMaxStackSize(StackingEffect effect) => serializableStackingEffectDictionary[effect].MaxStackSize;
    public float GetStackingEffectDuration(StackingEffect effect) => serializableStackingEffectDictionary[effect].Duration;

    private void Validate()
    {
        EffectLookupValidator effectLookupValidator = new EffectLookupValidator();

        effectLookupValidator.ValidateActiveEffects(serializableActiveEffectDictionary);
        effectLookupValidator.ValidatePassiveEffects(serializablePassiveEffectDictionary);
        effectLookupValidator.ValidateStackingEffects(serializableStackingEffectDictionary);

        if (effectLookupValidator.HasErrors)
        {
            Debug.LogError("Effect lookup errors found, see above for errors.");
        }
    }
}
