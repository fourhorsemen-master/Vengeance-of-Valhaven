using System.Collections.Generic;
using System.Linq;

public class AbilityRoomSaveData
{
    public List<Ability> AbilityChoices { get; set; } = new List<Ability>();
    public bool AbilitiesViewed { get; set; } = false;
    public bool AbilitySelected { get; set; } = false;

    public SerializableAbilityRoomSaveData Serialize()
    {
        return new SerializableAbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices.Select(x => x.Serialize()).ToList(),
            AbilitiesViewed = AbilitiesViewed,
            AbilitySelected = AbilitySelected
        };
    }
}
