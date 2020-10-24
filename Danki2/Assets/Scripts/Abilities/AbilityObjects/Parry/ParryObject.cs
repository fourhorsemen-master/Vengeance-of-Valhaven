using UnityEngine;

public class ParryObject : StaticAbilityObject
{
    [SerializeField]
    private ParryVisual parryVisualPrefab = null;

    public override float StickTime => 3;

    public static void Create(Transform transform, Subject onParry)
    {
        ParryObject parryObject = Instantiate(AbilityObjectPrefabLookup.Instance.ParryObjectPrefab, transform);
        onParry.Subscribe(parryObject.HandleParry);
    }

    private void HandleParry()
    {
        ParryVisual.Create(parryVisualPrefab, transform);
    }
}
