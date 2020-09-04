using UnityEngine;

public class BandageObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource bandageSound = null;

    private Quaternion forwardLookRotation;

    public static BandageObject Create(Transform casterTransform)
    {
        BandageObject bandageObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.BandageObjectPrefab,
            casterTransform.position,
            Quaternion.LookRotation(Vector3.forward, Vector3.up),
            casterTransform
        );

        bandageObject.forwardLookRotation = bandageObject.gameObject.transform.rotation;

        return bandageObject;
    }

    private void Update()
    {
        transform.rotation = forwardLookRotation;
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
