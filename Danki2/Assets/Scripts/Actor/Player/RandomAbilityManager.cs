using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RandomAbilityManager
{
    private readonly Player player;

    private readonly List<AbilityReference> availableAbilities = new List<AbilityReference>();

    private readonly ISet<AbilityReference> blockedAbilities = new HashSet<AbilityReference>
    {
        AbilityReference.Fireball,
        AbilityReference.Pounce,
        AbilityReference.Bite,
        AbilityReference.Slash
    };

    public RandomAbilityManager(Player player)
    {
        this.player = player;

        SetupAvailableAbilities();
        RoomManager.Instance.WaveStartSubject.Subscribe(OnWaveStart);
    }

    private void SetupAvailableAbilities()
    {
        EnumUtils.ForEach<AbilityReference>(abilityReference =>
        {
            if (blockedAbilities.Contains(abilityReference)) return;
            availableAbilities.Add(abilityReference);
        });
    }

    private void OnWaveStart(int wave)
    {
        if (wave == 1) return;
        
        player.AbilityTree.AddToInventory(GetRandomAbility());
    }

    private AbilityReference GetRandomAbility()
    {
        return availableAbilities[Random.Range(0, availableAbilities.Count)];
    }
}
