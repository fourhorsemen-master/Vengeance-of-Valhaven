using UnityEngine;

public class VisualSettings : Singleton<VisualSettings>
{
    [SerializeField]
    Color energyColour = Color.white;

    public Color EnergyColour => energyColour;
}
