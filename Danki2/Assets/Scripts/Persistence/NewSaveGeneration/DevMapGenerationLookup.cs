using UnityEngine;

public class DevMapGenerationLookup : MapGenerationLookup
{
    [SerializeField]
    private RoomsPerZoneLookup roomsPerZoneLookupOverride = new RoomsPerZoneLookup(0);
    
    public override RoomsPerZoneLookup RoomsPerZoneLookup { get => roomsPerZoneLookupOverride; set => roomsPerZoneLookupOverride = value; }

    protected override bool DestroyOnLoad => true;
}
