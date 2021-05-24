using UnityEngine;

public class ForestGolem : Enemy
{
    [SerializeField] private ForestGolemRootIndicator rootIndicatorPrefab = null;
    
    public override ActorType Type => ActorType.ForestGolem;

    public void BeginRootStorm()
    {
        this.ActOnInterval(1, _ =>
        {
            
        });
    }

    public void EndRootStorm()
    {
        
    }
}
