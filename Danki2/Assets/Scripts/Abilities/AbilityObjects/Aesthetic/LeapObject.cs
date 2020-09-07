using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource leapSound = null;

    [SerializeField]
    private AudioSource momentumSound = null;

    [SerializeField]
    private GameObject visual = null;

    public override float StickTime => 2;

    public static LeapObject Create(Transform casterTransform, Subject leapEndSubject)
    {
        LeapObject leapObject = Instantiate(AbilityObjectPrefabLookup.Instance.LeapObjectPrefab, casterTransform);
        leapObject.Setup(leapEndSubject);
        return leapObject;
    }

    public void PlayMomentumSound()
    {
        leapSound.Stop();
        momentumSound.Play();
    }

    private void Setup(Subject leapEndSubject)
    {
        leapEndSubject.Subscribe(() =>
        {
            visual.gameObject.SetActive(true);
        });
    }
}
