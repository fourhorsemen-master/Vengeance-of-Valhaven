using UnityEngine;
using UnityEngine.Experimental.VFX;

public class WhirlwindObject : MonoBehaviour
{
    private const float soundInterval = 0.35f;
    private const float minVolume = 0.4f;
    private const float maxVolume = 1f;
    private const float particleDissapationPeriod = 0.4f;

    private bool isDissipating = false;

    private VisualEffect pfx;

    [SerializeField]
    private AudioSource whirlwindSound = null;

    public static WhirlwindObject Create(Transform casterTransform)
    {
        WhirlwindObject prefab = AbilityObjectPrefabLookup.Instance.WhirlwindObjectPrefab;
        return Instantiate(prefab, casterTransform);
    }

    public void DestroyWhirlwind()
    {
        pfx.Stop();
        whirlwindSound.Stop();
        isDissipating = true;
        Destroy(gameObject, particleDissapationPeriod);
    }

    public void Start()
    {

        pfx = gameObject.GetComponent<VisualEffect>();

        this.ActOnInterval(soundInterval, () =>
        {
            if (!isDissipating)
            { 
                whirlwindSound.volume = Random.Range(minVolume, maxVolume);
                whirlwindSound.Play();
            }
        });
    }
}
