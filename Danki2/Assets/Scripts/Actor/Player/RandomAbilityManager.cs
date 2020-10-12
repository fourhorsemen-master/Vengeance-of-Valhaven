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

    private AbilityReference lastAbility = AbilityReference.FanOfKnives;

    public RandomAbilityManager(Player player)
    {
        this.player = player;
        RoomManager.Instance.WaveStartSubject.Subscribe(OnWaveStart);
    }

    private void OnWaveStart(int wave)
    {
        if (wave % 2 == 1) return;
        
        player.AbilityTree.AddToInventory(GetRandomAbility());
    }

    private AbilityReference GetRandomAbility()
    {
        blockedAbilities.Add(lastAbility);
        SetupAvailableAbilities();
        AbilityReference nextAbility = availableAbilities[Random.Range(0, availableAbilities.Count)];
        blockedAbilities.Remove(lastAbility);
        lastAbility = nextAbility;
        return nextAbility;
    }

    private void SetupAvailableAbilities()
    {
        availableAbilities.Clear();
        
        EnumUtils.ForEach<AbilityReference>(abilityReference =>
        {
            if (blockedAbilities.Contains(abilityReference)) return;
            availableAbilities.Add(abilityReference);
        });
    }
}
