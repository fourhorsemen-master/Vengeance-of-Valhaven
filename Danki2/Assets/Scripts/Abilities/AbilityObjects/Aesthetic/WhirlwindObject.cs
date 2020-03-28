using System.Collections;
using UnityEngine;

public class WhirlwindObject : MonoBehaviour
{
    private const float _soundInterval = 0.35f;
    private const float _minVolume = 0.4f;
    private const float _maxVolume = 1f;

    public AudioSource whirlwindSound = null;

    public static WhirlwindObject Create(Vector3 position, Quaternion rotation)
    {
        WhirlwindObject prefab = AbilityObjectPrefabLookup.Instance.WhirlwindObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    public void Start()
    {
        this.ActOnInterval(_soundInterval, () => 
        {
            whirlwindSound.volume = Random.Range(_minVolume, _maxVolume);
            whirlwindSound.Play();
        });
    }
}
