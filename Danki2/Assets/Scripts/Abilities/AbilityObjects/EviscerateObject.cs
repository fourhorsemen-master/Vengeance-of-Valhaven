using UnityEngine;

public class EviscerateObject : StaticAbilityObject
{
    [SerializeField]
    private Color slashColor = new Color();

    public override float StickTime => 5f;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        EviscerateObject prefab = AbilityObjectPrefabLookup.Instance.EviscerateObjectPrefab;
        EviscerateObject eviscerateObject = Instantiate(prefab, position, rotation);
        SlashObject.Create(position, rotation, eviscerateObject.slashColor);
    }
}
