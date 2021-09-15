using UnityEngine;

public class AbilityAnimationListener : MonoBehaviour
{
    public Subject SwingSubject { get; } = new Subject();
    public Subject ImpactSubject { get; } = new Subject();
    public Subject FinishSubject { get; } = new Subject();

    public void Swing() => SwingSubject.Next();

    public void Impact() => ImpactSubject.Next();

    public void Finish() => FinishSubject.Next();
}
