public class AbilityConstructionArgs
{
	public Actor Owner { get; }
	public AbilityData AbilityDataObject { get; }
	public string FmodVocalisationEvent { get; }
	public string FmodStartEvent { get; }
	public string FmodEndEvent { get; }
	public string[] ActiveBonuses { get; }
	public AbilityAnimationType Animation { get; }
	public float ChannelDuration { get; }

	public AbilityConstructionArgs
	(
		Actor owner,
		AbilityData abilityData,
		string fmodVocalisationEvent,
		string fmodStartEvent,
		string fmodEndEvent,
		string[] activeBonuses,
		AbilityAnimationType animationType,
		float channelDuration = -1f
	)
	{
		Owner = owner;
		AbilityDataObject = abilityData;
		FmodVocalisationEvent = fmodVocalisationEvent;
		FmodStartEvent = fmodStartEvent;
		FmodEndEvent = fmodEndEvent;
		ActiveBonuses = activeBonuses;
		Animation = animationType;
		ChannelDuration = channelDuration;
	}
}
