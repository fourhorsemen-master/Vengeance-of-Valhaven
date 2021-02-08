using System;
using System.Linq;
using UnityEngine;

public class HealthBasedRandomSelection<TState> : IStateMachineDecider<TState> where TState : Enum
{
    private readonly Actor actor;
    private readonly TState[][] statesPerHealthRegion;

    /// <summary>
    /// Returns a random state based on sets of choices for different health regions.
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="statesPerHealthRegion">
    ///     A state array for each health amount. Eg. if [[s1, s2], [s3, s4]] are passed in:
    ///     - At 50-0% health, either s1 or s2 will be chosen
    ///     - At 100-50% health, either s3 or s4 will be chosen
    /// </param>
    public HealthBasedRandomSelection(Actor actor, params TState[][] statesPerHealthRegion)
    {
        this.actor = actor;
        this.statesPerHealthRegion = statesPerHealthRegion;

        if (!statesPerHealthRegion.Any() || statesPerHealthRegion.Any(s => !s.Any()))
        {
            Debug.LogError($"Empty decision state list in AI for actor of type {nameof(actor)}");
        }
    }

    public TState Decide()
    {
        int numHealthRegions = statesPerHealthRegion.Length;

        int currentHealthRegion = actor.HealthManager.HealthProportion == 1
            ? numHealthRegions - 1
            : (int)(actor.HealthManager.HealthProportion * numHealthRegions);

        TState[] possibleStates = statesPerHealthRegion[currentHealthRegion];

        return RandomUtils.Choice(possibleStates);
    }
}
