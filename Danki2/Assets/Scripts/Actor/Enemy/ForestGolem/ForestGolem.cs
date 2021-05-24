using UnityEngine;

public class ForestGolem : Enemy
{
    [Header("Root")]
    [SerializeField] private ForestGolemRootIndicator rootIndicatorPrefab = null;
    [SerializeField] private ForestGolemRoot rootPrefab = null;
    [SerializeField] private float rootDelay = 0;
    [SerializeField] private float rootRange = 0;
    [SerializeField] private int rootDamage = 0;
    
    public override ActorType Type => ActorType.ForestGolem;

    public void FireRoot(Vector3 position)
    {
        Instantiate(rootIndicatorPrefab, position, Quaternion.identity);
        this.WaitAndAct(rootDelay, () =>
        {
            Instantiate(rootPrefab, position, Quaternion.identity);
            HandleRootDamage(position);
        });
    }

    private void HandleRootDamage(Vector3 position)
    {
        Player player = ActorCache.Instance.Player;
        if (Vector3.Distance(player.transform.position, position) > rootRange) return;

        player.HealthManager.ReceiveDamage(rootDamage, this);
    }
}
