using System.Collections.Generic;
using UnityEngine;

public class NewSaveGenerator2 : Singleton<NewSaveGenerator2>
{
    private const int SaveDataVersion = 0;

    protected override bool DestroyOnLoad => false;

    public void GenerateNextLayer(SaveData2 saveData)
    {
        Random.state = saveData.RandomState;
        MapGenerator2.Instance.GenerateNextLayer(saveData.CurrentRoomNode);
        saveData.RandomState = Random.state;
    }

    public SaveData2 Generate(int seed = -1)
    {
        if (seed == -1) seed = RandomUtils.Seed();
        Random.InitState(seed);
        
        SaveData2 saveData = new SaveData2
        {
            Version = SaveDataVersion,
            Seed = seed,
            PlayerHealth = 20,
            SerializableAbilityTree = GenerateAbilityTree().Serialize(),
            RuneSockets = GenerateRuneSockets(),
            RuneOrder = GenerateRuneOrder(),
            CurrentRoomNode = MapGenerator2.Instance.Generate(),
            DefeatRoom = GenerateDefeatRoom()
        };

        saveData.RandomState = Random.state;

        return saveData;
    }

    private AbilityTree GenerateAbilityTree()
    {
        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        ownedAbilities[AbilityReference.Slash] = 1;
        ownedAbilities[AbilityReference.Lunge] = 1;

        return AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.Slash),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );
    }

    private List<RuneSocket> GenerateRuneSockets()
    {
        List<RuneSocket> runeSockets = new List<RuneSocket>();
        Utils.Repeat(MapGenerationLookup.Instance.RuneSockets, () => runeSockets.Add(new RuneSocket()));
        return runeSockets;
    }

    private List<Rune> GenerateRuneOrder()
    {
        List<Rune> runes = EnumUtils.ToList<Rune>();
        runes.Shuffle();
        return runes;
    }

    private RoomNode GenerateDefeatRoom()
    {
        return new RoomNode
        {
            Scene = Scene.GameplayDefeatScene,
            RoomType = RoomType.Defeat
        };
    }
}
