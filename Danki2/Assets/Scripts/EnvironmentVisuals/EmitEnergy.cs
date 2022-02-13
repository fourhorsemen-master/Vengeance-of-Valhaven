using UnityEngine;

public class EmitEnergy : Emit
{
    protected override Color Colour => VisualSettings.Instance.EnergyColour;
}
