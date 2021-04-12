using UnityEngine;

public class MapGenerationLookup : Singleton<MapGenerationLookup>
{
    [SerializeField] private int abilityChoices = 0;
    
    [SerializeField] private int minRoomDepth = 0;
    [SerializeField] private int maxRoomDepth = 0;
    [SerializeField] private int minRoomExits = 0;
    [SerializeField] private int maxRoomExits = 0;

    public int AbilityChoices { get => abilityChoices; set => abilityChoices = value; }

    public int MinRoomDepth { get => minRoomDepth; set => minRoomDepth = value; }
    public int MaxRoomDepth { get => maxRoomDepth; set => maxRoomDepth = value; }
    public int MinRoomExits { get => minRoomExits; set => minRoomExits = value; }
    public int MaxRoomExits { get => maxRoomExits; set => maxRoomExits = value; }

    protected override bool DestroyOnLoad => false;
}
