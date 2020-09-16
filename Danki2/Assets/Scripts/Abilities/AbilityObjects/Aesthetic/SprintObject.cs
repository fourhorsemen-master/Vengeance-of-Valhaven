using UnityEngine;

public class SprintObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource sprintSound = null;

    public static void Create(Transform transform, Subject onCastCancelled, Subject onCastEnd)
    {
        SprintObject sprintObject = Instantiate(AbilityObjectPrefabLookup.Instance.SprintObjectPrefab, transform);
        onCastCancelled.Subscribe(sprintObject.Destroy);
        onCastEnd.Subscribe(sprintObject.HandleCastEnd);
    }

    private void HandleCastEnd()
    {
        sprintSound.Play();
        this.WaitAndAct(sprintSound.clip.length, Destroy);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
