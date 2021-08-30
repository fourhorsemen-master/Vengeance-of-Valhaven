using System.Collections.Generic;

public class AbilityRoomSaveData
{
    public List<SerializableGuid> AbilityChoices { get; set; } = new List<SerializableGuid>();
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
