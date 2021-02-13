using UnityEngine;

public class SweepingStrikeObject : StaticAbilityObject
{
    [SerializeField]
    private Color slashColor = new Color();

    public override float StickTime => 5f;

    public static SweepingStrikeObject Create(Vector3 position, Quaternion rotation)
    {
        SweepingStrikeObject prefab = AbilityObjectPrefabLookup.Instance.SweepingStrikeObjectPrefab;
        SweepingStrikeObject sweepingStrikeObject = Instantiate(prefab, position, rotation);
        SlashObject.Create(position, rotation, sweepingStrikeObject.slashColor);

        return sweepingStrikeObject;
    }
}
