﻿using System.Collections.Generic;
using System.Linq;
using UnityScene = UnityEngine.SceneManagement.Scene;

/// <summary>
/// Tracks whether the scene can be transitioned through. By default, the scene cannot be transitioned through.
/// When ALL registered subjects have emitted (i.e. indicated that they think the scene can be transitioned
/// through) then this class indicates that we can transition.
/// </summary>
public class GameplaySceneTransitionManager : Singleton<GameplaySceneTransitionManager>
{
    public BehaviourSubject<bool> CanTransitionSubject { get; } = new BehaviourSubject<bool>(false);
    public bool CanTransition => CanTransitionSubject.Value;

    private readonly Dictionary<Subject, bool> canTransitionSubjects = new Dictionary<Subject, bool>();

    public void RegisterCanTransitionSubject(Subject canTransitionSubject)
    {
        canTransitionSubjects[canTransitionSubject] = false;
        canTransitionSubject.Subscribe(() =>
        {
            canTransitionSubjects[canTransitionSubject] = true;
            if (canTransitionSubjects.Values.All(c => c)) CanTransitionSubject.Next(true);
        });
    }
}
