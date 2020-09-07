using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource leapSound = null;

    [SerializeField]
    private AudioSource hitSound = null;

    [SerializeField]
    private GameObject landingVisual = null;

    public override float StickTime => 2;

    private Transform casterTransform;
    private bool hasMomentum;

    public static LeapObject Create(Transform casterTransform, Subject leapEndSubject, bool hasMomentum)
    {
        LeapObject leapObject = Instantiate(AbilityObjectPrefabLookup.Instance.LeapObjectPrefab, casterTransform.position, casterTransform.rotation);

        leapObject.casterTransform = casterTransform;
        leapObject.hasMomentum = hasMomentum;
        leapObject.Setup(leapEndSubject);

        return leapObject;
    }

    public void PlayHitSound()
    {
        leapSound.Stop();
        hitSound.Play();
    }

    private void Setup(Subject leapEndSubject)
    {
        leapEndSubject.Subscribe(() =>
        {
            Vector3 position = casterTransform.position;

            if (hasMomentum)
            {
                SmashObject.CreateWithoutSound(position);
                return;
            }

            gameObject.transform.position = position;
            landingVisual.gameObject.SetActive(true);
        });
    }
}
