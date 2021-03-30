using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationStringLookup
{
	public static Dictionary<AbilityAnimationType, string> LookupTable = new Dictionary<AbilityAnimationType, string>() 
	{
		{ AbilityAnimationType.None, "InvalidAnimType" },
		{ AbilityAnimationType.Slash, "Slash_OneShot" },
		{ AbilityAnimationType.OverheadSlash, "Slash_Down_OneShot" },
		{ AbilityAnimationType.Lunge, "Lunge_OneShot" },
		{ AbilityAnimationType.GroundSlam, "GroundSlam_OneShot" }
	};
}