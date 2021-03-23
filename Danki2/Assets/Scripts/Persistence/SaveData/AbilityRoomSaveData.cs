using System.Collections.Generic;

public class AbilityRoomSaveData
{
    public List<AbilityReference> AbilityChoices { get; set; } = new List<AbilityReference>();
    public bool AbilitiesViewed { get; set; } = false;

    public SerializableAbilityRoomSaveData Serialize()
    {
        return new SerializableAbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices,
            AbilitiesViewed = AbilitiesViewed
        };
    }
}
