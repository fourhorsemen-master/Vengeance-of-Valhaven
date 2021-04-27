using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class NewSaveGeneratorTest
{
    [OneTimeSetUp]
    public void SetUp()
    {
        TestUtils.InstantiatePrefab<AbilityLookup>(TestUtils.AbilityLookupAssetPath);
        TestUtils.InstantiatePrefab<RarityLookup>(TestUtils.RarityLookupAssetPath);
        TestUtils.InstantiatePrefab<MapGenerationLookup>(TestUtils.MapGenerationLookupAssetPath);
        TestUtils.InstantiatePrefab<SceneLookup>(TestUtils.SceneLookupAssetPath);
        TestUtils.InstantiatePrefab<MapGenerator>(TestUtils.MapGeneratorAssetPath);
        TestUtils.InstantiatePrefab<NewSaveGenerator>(TestUtils.NewSaveGeneratorAssetPath);
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
    public IEnumerator TestGenerateProducesDifferentSaveDataWithDifferentSeeds()
    {
        int seed = RandomUtils.Seed();
        SaveData saveData1 = NewSaveGenerator.Instance.Generate(seed);
        SaveData saveData2 = NewSaveGenerator.Instance.Generate(seed + 1);
        
        TestUtils.AssertNotEqualByJson<SaveData, SerializableSaveData>(saveData1, saveData2);
        
        yield return null;
    }
}
