public class AbilityConstructionArgs
{
	public Player Owner { get; }
	public AbilityData AbilityDataObject { get; }
	public AbilityVocalisationType AbilityVocalisationType { get; }
	public string FmodStartEvent { get; }
	public string FmodEndEvent { get; }
	public string[] ActiveBonuses { get; }
	public AbilityAnimationType Animation { get; }
	public float ChannelDuration { get; }

	public AbilityConstructionArgs
	(
		Player owner,
		AbilityData abilityData,
		AbilityVocalisationType abilityVocalisationType,
		string fmodStartEvent,
		string fmodEndEvent,
		string[] activeBonuses,
		AbilityAnimationType animationType,
		float channelDuration = -1f
	)
	{
		Owner = owner;
		AbilityDataObject = abilityData;
		AbilityVocalisationType = abilityVocalisationType;
		FmodStartEvent = fmodStartEvent;
		FmodEndEvent = fmodEndEvent;
		ActiveBonuses = activeBonuses;
		Animation = animationType;
		ChannelDuration = channelDuration;
	}
}
