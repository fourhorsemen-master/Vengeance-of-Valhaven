using UnityEngine;

public class AbilityAnimationListener : MonoBehaviour
{
    public Subject ImpactSubject { get; } = new Subject();

    public void Impact()
    {
        ImpactSubject.Next();
    }
}
