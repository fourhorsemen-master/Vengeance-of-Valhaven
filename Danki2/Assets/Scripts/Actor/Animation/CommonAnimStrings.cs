public static class CommonAnimStrings
{
	public const string
		Locomotion = "Locomotion_Blend",
		ForwardBlend = "Forward_MoveSpeed_Blend",
		StrafeBlend = "Lateral_MoveSpeed_Blend",
		Interrupted = "Interrupted_OneShot",
		Dash = "Dash_OneShot",
		FlinchLeft = "Additive_Flinch_Left",
		FlinchRight = "Additive_Flinch_Right";

	public static string RandomFlinch => RandomUtils.Choice(FlinchLeft, FlinchRight);
}
