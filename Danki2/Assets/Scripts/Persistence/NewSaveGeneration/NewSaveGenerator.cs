using System.Collections.Generic;
using UnityEngine;

public class NewSaveGenerator : Singleton<NewSaveGenerator>
{
    private const int SaveDataVersion = 0;

    protected override bool DestroyOnLoad => false;

    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    /// <param name="seed"> An optional seed to generate the save with </param>
    public SaveData Generate(int seed = -1)
    {
        if (seed == -1) seed = RandomUtils.Seed();
        Random.InitState(seed);
        
        SaveData saveData = new SaveData
        {
            Version = SaveDataVersion,
            Seed = seed,
            PlayerHealth = 30,
            CurrencyAmount = 0,
            CurrentRoomNode = MapGenerator.Instance.Generate()
        };

        SetAbilityTree(saveData);
        SetRuneSockets(saveData);
        SetRuneOrder(saveData);
        SetDefeatRoom(saveData);

        saveData.RandomState = Random.state;

        return saveData;
    }

    public void GenerateNextLayer(SaveData saveData)
    {
        Random.state = saveData.RandomState;
        MapGenerator.Instance.GenerateNextLayer(saveData.CurrentRoomNode);
        saveData.RandomState = Random.state;
    }
    
    private void SetAbilityTree(SaveData saveData)
    {
        List<Ability> ownedAbilities = new List<Ability>();

        Ability leftStartingAbility = CustomAbilityLookup.Instance.GetByName(
            MapGenerationLookup.Instance.LeftStartingAbilityName
        );

        Ability rightStartingAbility = CustomAbilityLookup.Instance.GetByName(
            MapGenerationLookup.Instance.RightStartingAbilityName
        );

        ownedAbilities.Add(leftStartingAbility);
        ownedAbilities.Add(rightStartingAbility);

        saveData.SerializableAbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(leftStartingAbility),
            AbilityTreeFactory.CreateNode(rightStartingAbility)
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
