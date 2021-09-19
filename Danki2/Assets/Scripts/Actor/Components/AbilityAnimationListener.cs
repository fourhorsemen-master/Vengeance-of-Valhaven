using UnityEngine;

public class AbilityAnimationListener : MonoBehaviour
{
    public Subject StartSubject { get; } = new Subject();
    public Subject SwingSubject { get; } = new Subject();
    public Subject ImpactSubject { get; } = new Subject();
    public Subject FinishSubject { get; } = new Subject();

    public void AbilityStart() => StartSubject.Next();

    public void AbilitySwing() => SwingSubject.Next();

    public void AbilityImpact() => ImpactSubject.Next();

    public void AbilityFinish() => FinishSubject.Next();
}
