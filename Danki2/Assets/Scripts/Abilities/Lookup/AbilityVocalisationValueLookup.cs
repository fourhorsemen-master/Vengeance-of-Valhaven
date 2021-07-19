using System.Collections.Generic;

public static class AbilityVocalisationValueLookup
{
	public static Dictionary<AbilityVocalisationType, float> LookupTable = new Dictionary<AbilityVocalisationType, float>()
	{
		{ AbilityVocalisationType.None, default },
		{ AbilityVocalisationType.Small, 0f },
		{ AbilityVocalisationType.Medium, 1f }
	};
}
