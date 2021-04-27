using UnityEngine;

public class WraithSwipeObject : StaticAbilityObject
{
    [SerializeField]
    private Color slashColor = new Color();

    public override float StickTime => 5f;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        WraithSwipeObject prefab = AbilityObjectPrefabLookup.Instance.WraithSwipeObjectPrefab;
        WraithSwipeObject wraithSwipeObject = Instantiate(prefab, position, rotation);
        SlashObject.Create(position, rotation, wraithSwipeObject.slashColor);
    }
}
