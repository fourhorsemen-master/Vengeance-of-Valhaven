using UnityEngine;

internal class EmitColour : Emit
{
    [SerializeField] private Color colour = default;

    protected override Color Colour => colour;
}