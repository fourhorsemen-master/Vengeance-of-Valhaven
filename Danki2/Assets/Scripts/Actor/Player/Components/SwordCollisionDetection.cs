using System.Linq;
using UnityEngine;

public class SwordCollisionDetection : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private bool isDetecting = false;

    private void Start()
    {
        player.AbilityAnimationListener.SwingSubject.Subscribe(StartCollisionDetection);
        player.AbilityAnimationListener.ImpactSubject.Subscribe(StopCollisionDetection);
    }

    private void StartCollisionDetection() => isDetecting = true;

    private void StopCollisionDetection() => isDetecting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldTriggerCollisionSound(other))
        {
            CollisionSoundManager.Instance.Play(other.sharedMaterial, CollisionSoundLevel.High, transform.position);
        }
    }

    private bool ShouldTriggerCollisionSound(Collider other)
    {
        if (!isDetecting) return false;

        if (other.sharedMaterial == null) return false;

        if (player.Colliders.Contains(other)) return false;

        if (other.gameObject.layer == (int)Layer.Actors) return false;

        return true;
    }
}
