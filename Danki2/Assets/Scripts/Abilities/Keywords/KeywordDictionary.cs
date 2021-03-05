using System;

[Serializable]
public class KeywordDictionary : SerializableEnumDictionary<Keyword, SerializableKeywordData>
{
    public KeywordDictionary(SerializableKeywordData defaultValue) : base(defaultValue)
    {
    }

    public KeywordDictionary(Func<SerializableKeywordData> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}
