using System;
using UnityEngine;

public class AbilityIconManager : Singleton<AbilityIconManager>
{
    public AbilityIconDictionary spriteLookup = new AbilityIconDictionary((Sprite)null);

    protected override void Awake()
    {
        base.Awake();

        foreach (AbilityReference template in Enum.GetValues(typeof(AbilityReference)))
        {
            Sprite sprite = spriteLookup[template];
            if (sprite == null)
            {
                Debug.LogError($"No icon found for template: {template.ToString()}");
                continue;
            }
        }
    }

    public Sprite GetIcon(AbilityReference ability)
    {
        return spriteLookup[ability];
    }
}
