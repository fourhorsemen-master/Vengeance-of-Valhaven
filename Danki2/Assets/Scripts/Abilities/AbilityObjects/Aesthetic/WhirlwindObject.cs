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
        StartCoroutine(PlaySoundAndWait(_soundInterval));
    }

    private IEnumerator PlaySoundAndWait(float soundInterval)
    {
        while(true)
        {
            whirlwindSound.volume = Random.Range(_minVolume, _maxVolume);
            whirlwindSound.Play();
            yield return new WaitForSeconds(soundInterval);
        }
    }
}
