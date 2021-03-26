using UnityEngine;

public class RarityLookup : Singleton<RarityLookup>
{
    [SerializeField]
    private RarityDictionary lookup = new RarityDictionary(() => new RarityData());

    public RarityDictionary Lookup => lookup;
}
