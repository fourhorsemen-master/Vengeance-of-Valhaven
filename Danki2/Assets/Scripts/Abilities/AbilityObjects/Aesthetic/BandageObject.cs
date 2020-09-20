using UnityEngine;

public class BandageObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource bandageSound = null;

    public static BandageObject Create(Transform casterTransform)
    {
        BandageObject bandageObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.BandageObjectPrefab,
            casterTransform.position,
            Quaternion.LookRotation(Vector3.forward, Vector3.up),
            casterTransform
        );

        return bandageObject;
    }

    public void PlaySound()
    {
        bandageSound.Play();
    }

    public void Destroy()
    {
        this.WaitAndAct(bandageSound.clip.length, () => Destroy(gameObject));
    }
}
