using UnityEngine;

[Behaviour("Attack Player", AIAction.Attack)]
public class AttackPlayer<T> : IBehaviour<T> where T : Actor
{
    public void Behave(T actor)
    {
        Debug.Log("Attacking player...");
    }
}
