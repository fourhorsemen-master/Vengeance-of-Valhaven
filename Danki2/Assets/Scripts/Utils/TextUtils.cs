public class TextUtils
{
    /// <summary>
    /// Adds colour tags that unity uses to apply colour to the string. Note that RichText will need to be active on the Text component for this to work.
    /// </summary>
    /// <param name="colourHexCode">6 character hex code (ie. no hash character)</param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ColouredText(string colourHexCode, string text)
    {
        return $"<color=#{colourHexCode}>{text}</color>";
    }
}
