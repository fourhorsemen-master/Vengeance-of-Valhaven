using UnityEngine;

public class WolfAttack2 : IAiComponent
{
    public void Enter()
    {
        Debug.Log("Entering attack");
    }

    public void Exit()
    {
        Debug.Log("Exiting attack");
    }

    public void Update()
    {
        Debug.Log("Updating attack");
    }
}
