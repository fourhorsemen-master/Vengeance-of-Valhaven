using System.Collections.Generic;
using System.Linq;
using UnityScene = UnityEngine.SceneManagement.Scene;

public class GameplaySceneTransitionManager : Singleton<GameplaySceneTransitionManager>
{
    public bool CanTransition { get; private set; } = false;

    private readonly Dictionary<Subject, bool> canTransitionSubjects = new Dictionary<Subject, bool>();

    public void RegisterCanTransitionSubject(Subject canTransitionSubject)
    {
        canTransitionSubjects[canTransitionSubject] = false;
        canTransitionSubject.Subscribe(() =>
        {
            canTransitionSubjects[canTransitionSubject] = true;
            if (canTransitionSubjects.Values.All(c => c)) CanTransition = true;
        });
    }
}
