using UnityEngine;

public class SprintObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource sprintSound = null;

    public override float StickTime => sprintSound.clip.length;

    public static void Create(Transform transform)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.SprintObjectPrefab, transform);
    }
}
