using UnityEngine;

public class PoisonStabVisual : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private ModularPFXComponent mpfx;
    public ModularPFXComponent Mpfx => mpfx;

    public static PoisonStabVisual Create(PoisonStabVisual prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }
}
