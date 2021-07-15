public class AbilityConstructionArgs
{
	public Actor Owner { get; }
	public AbilityData AbilityDataObject { get; }
	public string FmodStartEvent { get; }
	public string FmodEndEvent { get; }
	public string[] ActiveBonuses { get; }
	public AbilityAnimationType Animation { get; }
	public AbilityVocalisationType AbilityVocalisationType { get; }
	public float ChannelDuration { get; }

	public AbilityConstructionArgs
	(
		Actor owner,
		AbilityData abilityData,
		string fmodStartEvent,
		string fmodEndEvent,
		string[] activeBonuses,
		AbilityAnimationType animationType,
		AbilityVocalisationType abilityVocalisationType,
		float channelDuration = -1f
	)
	{
		Owner = owner;
		AbilityDataObject = abilityData;
		FmodStartEvent = fmodStartEvent;
		FmodEndEvent = fmodEndEvent;
		ActiveBonuses = activeBonuses;
		Animation = animationType;
		AbilityVocalisationType = abilityVocalisationType;
		ChannelDuration = channelDuration;
	}
}
