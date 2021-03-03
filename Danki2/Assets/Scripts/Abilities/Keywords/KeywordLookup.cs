using UnityEngine;

public class KeywordLookup : Singleton<KeywordLookup>
{
    public KeywordDictionary keywordLookup = new KeywordDictionary(() => new SerializableKeywordData());

    protected override void Awake()
    {
        base.Awake();

        EnumUtils.ForEach<Keyword>(k =>
        {
            if (keywordLookup[k] == null)
            {
                Debug.LogError($"No data found for keyword: {k.ToString()}");
            }
        });
    }

    public string GetDisplayName(Keyword keyword) => keywordLookup[keyword].DisplayName;

    public string GetDescription(Keyword keyword) => keywordLookup[keyword].Description;
}
