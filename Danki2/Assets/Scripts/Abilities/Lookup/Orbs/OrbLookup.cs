using System;
using UnityEngine;

public class OrbLookup : Singleton<OrbLookup>
{
    public OrbDisplayNameMap displayNameMap = new OrbDisplayNameMap(string.Empty);
    public OrbColourMap colourMap = new OrbColourMap(Color.white);
    public OrbSpriteMap spriteMap = new OrbSpriteMap(defaultValue: null);

    protected override void Awake()
    {
        base.Awake();

        foreach (OrbType orbType in Enum.GetValues(typeof(OrbType)))
        {
            if (string.IsNullOrWhiteSpace(displayNameMap[orbType]))
            {
                Debug.LogError($"No display name set for orb type: {orbType.ToString()}");
            }

            if (colourMap[orbType] == Color.white)
            {
                Debug.LogError($"Default colour set for ability orb type: {orbType.ToString()}");
            }

            if (spriteMap[orbType] == null)
            {
                Debug.LogError($"No icon found for orb type: {orbType.ToString()}");
            }
        }
    }
    public string GetDisplayName(OrbType orbType) => displayNameMap[orbType];

    public Color GetColour(OrbType orbType) => colourMap[orbType];

    public Sprite GetSprite(OrbType orbType) => spriteMap[orbType];
}
