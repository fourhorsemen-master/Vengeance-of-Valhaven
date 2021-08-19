using System.Collections.Generic;

public class AbilityRoomSaveData
{
    public List<Ability2> AbilityChoices { get; set; } = new List<Ability2>();
    public bool AbilitiesViewed { get; set; } = false;
    public bool AbilitySelected { get; set; } = false;

    public SerializableAbilityRoomSaveData Serialize()
    {
        return new SerializableAbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices,
            AbilitiesViewed = AbilitiesViewed,
            AbilitySelected = AbilitySelected
        };
    }
}
