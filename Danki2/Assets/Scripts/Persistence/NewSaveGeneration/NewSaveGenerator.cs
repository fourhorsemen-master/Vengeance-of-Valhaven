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
            PlayerHealth = 20,
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
        Dictionary<SerializableGuid, int> ownedAbilities = new Dictionary<SerializableGuid, int>();
        AbilityLookup2.Instance.ForEachAbilityId(abilityId => ownedAbilities[abilityId] = 0);

        if (!AbilityLookup2.Instance.TryGetAbilityId(MapGenerationLookup.Instance.LeftStartingAbilityName, out SerializableGuid leftAbilityId))
        {
            Debug.LogError($"Invalid left starting ability name: {MapGenerationLookup.Instance.LeftStartingAbilityName}.");
            return;
        }

        if (!AbilityLookup2.Instance.TryGetAbilityId(MapGenerationLookup.Instance.RightStartingAbilityName, out SerializableGuid rightAbilityId))
        {
            Debug.LogError($"Invalid right starting ability name: {MapGenerationLookup.Instance.RightStartingAbilityName}.");
            return;
        }

        ownedAbilities[leftAbilityId]++;
        ownedAbilities[rightAbilityId]++;

        saveData.SerializableAbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(leftAbilityId),
            AbilityTreeFactory.CreateNode(rightAbilityId)
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
