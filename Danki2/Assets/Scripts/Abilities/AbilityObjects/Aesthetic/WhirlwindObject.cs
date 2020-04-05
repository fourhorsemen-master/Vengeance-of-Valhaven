using UnityEngine;
using UnityEngine.Experimental.VFX;

public class WhirlwindObject : MonoBehaviour
{
    private const float _soundInterval = 0.35f;
    private const float _minVolume = 0.4f;
    private const float _maxVolume = 1f;
    private const float _particleDissapationPeriod = 0.4f;

    private bool _isDissapating = false;

    private VisualEffect _pfx;

    public AudioSource whirlwindSound = null;

    public static WhirlwindObject Create(Transform casterTransform)
    {
        WhirlwindObject prefab = AbilityObjectPrefabLookup.Instance.WhirlwindObjectPrefab;
        return Instantiate(prefab, casterTransform);
    }

    public void DestroyWhirlwind()
    {
        _pfx.Stop();
        whirlwindSound.Stop();
        _isDissapating = true;
        Destroy(gameObject, _particleDissapationPeriod);
    }

    public void Start()
    {

        _pfx = gameObject.GetComponent<VisualEffect>();

        this.ActOnInterval(_soundInterval, () =>
        {
            if (!_isDissapating)
            { 
                whirlwindSound.volume = Random.Range(_minVolume, _maxVolume);
                whirlwindSound.Play();
            }
        });
    }
}
