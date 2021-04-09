using System;
using UnityEngine;

[Serializable]
public class MenuData
{
    [SerializeField] private GameplayState gameplayState;
    [SerializeField] private GameObject menu;

    public GameplayState GameplayState { get => gameplayState; set => gameplayState = value; }
    public GameObject Menu { get => menu; set => menu = value; }
}
