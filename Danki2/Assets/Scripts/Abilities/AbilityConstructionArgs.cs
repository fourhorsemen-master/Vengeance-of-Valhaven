public class AbilityConstructionArgs
{
	public Actor Owner { get; }
	public AbilityData AbilityDataObject { get; }
	public string FmodStartEvent { get; }
	public string FmodEndEvent { get; }
	public string[] ActiveBonuses { get; }
	public AbilityAnimationType Animation { get; }
	public float ChannelDuration { get; }

	public AbilityConstructionArgs
	(
		Actor owner,
		AbilityData abilityData,
		string fmodStartEvent,
		string fmodEndEvent,
		string[] activeBonuses,
		AbilityAnimationType animationType,
		float channelDuration = -1f
	)
	{
		this.Owner = owner;
		this.AbilityDataObject = abilityData;
		this.FmodStartEvent = fmodStartEvent;
		this.FmodEndEvent = fmodEndEvent;
		this.ActiveBonuses = activeBonuses;
		this.Animation = animationType;
		this.ChannelDuration = channelDuration;
	}
}
