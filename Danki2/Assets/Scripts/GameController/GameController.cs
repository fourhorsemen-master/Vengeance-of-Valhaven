using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public GameState GameState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        GameState = GameState.Playing;
    }
}
