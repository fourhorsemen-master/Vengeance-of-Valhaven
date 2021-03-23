using System.Collections.Generic;

public class AbilityRoomSaveData
{
    public List<AbilityReference> AbilityChoices { set; get; } = new List<AbilityReference>();

    public SerializableAbilityRoomSaveData Serialize()
    {
        return new SerializableAbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices
        };
    }
}
