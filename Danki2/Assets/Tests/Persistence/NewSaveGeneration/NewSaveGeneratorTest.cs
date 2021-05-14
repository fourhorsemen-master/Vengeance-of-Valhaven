using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class NewSaveGeneratorTest
{
    [OneTimeSetUp]
    public void SetUp()
    {
        TestUtils.InstantiatePrefab<AbilityLookup>();
        TestUtils.InstantiatePrefab<RarityLookup>();
        TestUtils.InstantiatePrefab<MapGenerationLookup>();
        TestUtils.InstantiatePrefab<SceneLookup>();
        TestUtils.InstantiatePrefab<MapGenerator>();
        TestUtils.InstantiatePrefab<NewSaveGenerator>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        AbilityLookup.Instance.Destroy();
        RarityLookup.Instance.Destroy();
        MapGenerationLookup.Instance.Destroy();
        SceneLookup.Instance.Destroy();
        MapGenerator.Instance.Destroy();
        NewSaveGenerator.Instance.Destroy();
    }
    
    [UnityTest]
    public IEnumerator TestGenerateProducesTheSameSaveDataWithTheSameSeed()
    {
        int seed = RandomUtils.Seed();
        SaveData saveData1 = NewSaveGenerator.Instance.Generate(seed);
        SaveData saveData2 = NewSaveGenerator.Instance.Generate(seed);
        
        TestUtils.AssertEqualByJson<SaveData, SerializableSaveData>(saveData1, saveData2);
        
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestGenerateWithMultipleLayersProducesTheSameSaveDataWithTheSameSeed()
    {
        int seed = RandomUtils.Seed();
        SaveData saveData1 = NewSaveGenerator.Instance.Generate(seed);
        SaveData saveData2 = NewSaveGenerator.Instance.Generate(seed);

        saveData1.CurrentRoomNode = saveData1.CurrentRoomNode.Children[0];
        NewSaveGenerator.Instance.GenerateNextLayer(saveData1);
        saveData2.CurrentRoomNode = saveData2.CurrentRoomNode.Children[0];
        NewSaveGenerator.Instance.GenerateNextLayer(saveData2);

        saveData1.CurrentRoomNode = saveData1.CurrentRoomNode.Children[0];
        NewSaveGenerator.Instance.GenerateNextLayer(saveData1);
        saveData2.CurrentRoomNode = saveData2.CurrentRoomNode.Children[0];
        NewSaveGenerator.Instance.GenerateNextLayer(saveData2);
        
        TestUtils.AssertEqualByJson<SaveData, SerializableSaveData>(saveData1, saveData2);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGenerateProducesDifferentSaveDataWithDifferentSeeds()
    {
        int seed = RandomUtils.Seed();
        SaveData saveData1 = NewSaveGenerator.Instance.Generate(seed);
        SaveData saveData2 = NewSaveGenerator.Instance.Generate(seed + 1);
        
        TestUtils.AssertNotEqualByJson<SaveData, SerializableSaveData>(saveData1, saveData2);
        
        yield return null;
    }
}
