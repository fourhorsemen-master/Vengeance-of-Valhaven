using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

public class GameplaySceneManager : Singleton<GameplaySceneManager>
{
    public bool CanTransition { get; private set; } = false;

    private readonly Dictionary<Subject, bool> canTransitionSubjects = new Dictionary<Subject, bool>();

    private void Update()
    {
        // TODO: hook up controls to real system.
        if (Input.GetKeyDown(KeyCode.Alpha1)) PersistenceManager.Instance.TransitionToNextScene(); // when the next scene is picked to transition to
        if (Input.GetKeyDown(KeyCode.Escape)) SceneUtils.LoadScene(Scene.GameplayExitScene); // to quit
    }

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
