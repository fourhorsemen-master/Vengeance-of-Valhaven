using System;
using UnityEngine;

public class AbilityIconManager : Singleton<AbilityIconManager>
{
    public AbilityIconDictionary spriteLookup = new AbilityIconDictionary((Sprite)null);

    protected override void Awake()
    {
        base.Awake();

        EnumUtils.ForEach<AbilityReference>(a =>
        {
            if (spriteLookup[a] == null)
            {
                Debug.LogError($"No icon found for ability: {a.ToString()}");
            }
        });
    }

    public Sprite GetIcon(AbilityReference ability)
    {
        return spriteLookup[ability];
    }
}
