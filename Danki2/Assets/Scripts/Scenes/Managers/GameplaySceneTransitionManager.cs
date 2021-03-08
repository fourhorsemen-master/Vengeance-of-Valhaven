using System.Collections.Generic;
using System.Linq;
using UnityScene = UnityEngine.SceneManagement.Scene;

/// <summary>
/// Tracks whether the scene can be transitioned through. By default, the scene cannot be transitioned through.
/// When ALL registered subjects have emitted (i.e. indicated that they think the scene can be transitioned
/// through) then this class indicates that we can transition.
/// </summary>
public class GameplaySceneTransitionManager : Singleton<GameplaySceneTransitionManager>
{
    public bool CanTransition { get; private set; } = false;
    
    public Subject CanTransitionSubject { get; } = new Subject();

    private readonly Dictionary<Subject, bool> canTransitionSubjects = new Dictionary<Subject, bool>();

    public void RegisterCanTransitionSubject(Subject canTransitionSubject)
    {
        canTransitionSubjects[canTransitionSubject] = false;
        canTransitionSubject.Subscribe(() =>
        {
            canTransitionSubjects[canTransitionSubject] = true;
            if (canTransitionSubjects.Values.All(c => c))
            {
                CanTransition = true;
                CanTransitionSubject.Next();
            }
        });
    }
}
