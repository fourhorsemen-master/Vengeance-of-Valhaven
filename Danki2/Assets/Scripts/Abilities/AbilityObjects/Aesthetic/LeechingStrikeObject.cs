using UnityEngine;

public class LeechingStrikeObject : MonoBehaviour
{
    // TODO : This ability object uses the SlashObject prefab material. This wants changing to a generic slash object where we pass
    //        through the colour and audiosource parameters.

    [SerializeField]
    private float rotationSpeed = 0f;

    [SerializeField]
    private float duration = 0f;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    [SerializeField]
    private AudioSource hitAudioSource = null;

    [SerializeField]
    private Color slashColor = new Color();

    public static LeechingStrikeObject Create(Vector3 position, Quaternion rotation)
    {
        LeechingStrikeObject prefab = AbilityObjectPrefabLookup.Instance.LeechingStrikeObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    private void Start()
    {
        meshRenderer.material.SetColor("_UnlitColor", slashColor);

        this.WaitAndAct(duration, () => Destroy(gameObject));
    }

    private void Update()
    {
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}
