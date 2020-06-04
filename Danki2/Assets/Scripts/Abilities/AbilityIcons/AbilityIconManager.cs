using System;
using UnityEngine;

public class AbilityIconManager : Singleton<AbilityIconManager>
{
    public AbilityIconDictionary spriteLookup = new AbilityIconDictionary((Sprite)null);

    protected override void Awake()
    {
        base.Awake();

        foreach (AbilityReference ability in Enum.GetValues(typeof(AbilityReference)))
        {
            if (spriteLookup[ability] == null)
            {
                Debug.LogError($"No icon found for ability: {ability.ToString()}");
            }
        }
    }

    public Sprite GetIcon(AbilityReference ability)
    {
        return spriteLookup[ability];
    }
}
