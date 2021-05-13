using System.Collections.Generic;
using UnityEngine;

public class NewSaveGenerator : Singleton<NewSaveGenerator>
{
    private const int SaveDataVersion = 0;

    protected override bool DestroyOnLoad => false;

    public SaveData Generate(int seed = -1)
    {
        if (seed == -1) seed = RandomUtils.Seed();
        Random.InitState(seed);
        
        SaveData saveData = new SaveData
        {
            Version = SaveDataVersion,
            Seed = seed,
            PlayerHealth = 20,
            CurrentRoomNode = MapGenerator.Instance.Generate()
        };

        SetAbilityTree(saveData);
        SetRuneSockets(saveData);
        SetRuneOrder(saveData);
        SetDefeatRoom(saveData);

        saveData.RandomState = Random.state;

        return saveData;
    }

    private void SetAbilityTree(SaveData saveData)
    {
        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        ownedAbilities[AbilityReference.Slash] = 1;
        ownedAbilities[AbilityReference.Lunge] = 1;

        saveData.SerializableAbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.Slash),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        ).Serialize();
    }

    private void SetRuneSockets(SaveData saveData)
    {
        Utils.Repeat(MapGenerationLookup.Instance.RuneSockets, () => saveData.RuneSockets.Add(new RuneSocket()));
    }

    private void SetRuneOrder(SaveData saveData)
    {
        EnumUtils.ForEach<Rune>(rune => saveData.RuneOrder.Add(rune));
        saveData.RuneOrder.Shuffle();
    }

    private void SetDefeatRoom(SaveData saveData)
    {
        saveData.DefeatRoom = new RoomNode
        {
            Scene = Scene.GameplayDefeatScene,
            RoomType = RoomType.Defeat
        };
    }
}
