using UnityEngine;

public class SlashObject : AbilityObject
{
    [SerializeField]
    private float rotationSpeed = 0f;

    [SerializeField]
    private float duration = 0f;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    [SerializeField]
    private Color desiredColor = new Color();

    public static SlashObject Create(Vector3 position, Quaternion rotation, Color color = default)
    {
        SlashObject prefab = AbilityObjectPrefabLookup.Instance.SlashObjectPrefab;
        SlashObject slashObject = Instantiate(prefab, position, rotation);
        if (!color.Equals(default)) slashObject.desiredColor = color;

        return slashObject;
    }

    private void Start()
    {
        meshRenderer.material.SetUnlitColour(desiredColor);

        this.WaitAndAct(duration, () => Destroy(gameObject));
    }

    private void Update()
    {
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
    }
}
