using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public static class TestUtils
{
    public static string AbilityLookupAssetPath => "Assets/Prefabs/Meta/GameplayManagers/AbilityLookup.prefab";
    public static string RarityLookupAssetPath => "Assets/Prefabs/Meta/GameplayManagers/RarityLookup.prefab";
    public static string MapGenerationLookupAssetPath => "Assets/Prefabs/Meta/MapGenerationLookup.prefab";
    public static string SceneLookupAssetPath => "Assets/Prefabs/Meta/SceneLookup.prefab";
    public static string MapGeneratorAssetPath => "Assets/Prefabs/Meta/MapGenerator.prefab";
    public static string NewSaveGeneratorAssetPath => "Assets/Prefabs/Meta/NewSaveGenerator.prefab";
    
    public static T InstantiatePrefab<T>(string assetPath) where T : Object
    {
        T prefab = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        return Object.Instantiate(prefab);
    }
    
    public static void AssertEqualByJson<T, U>(T expected, T actual) where T : ISerializable<U>
    {
        string expectedJson = JsonUtility.ToJson(expected.Serialize());
        string actualJson = JsonUtility.ToJson(actual.Serialize());
        Assert.AreEqual(expectedJson, actualJson);
    }
    
    public static void AssertNotEqualByJson<T, U>(T expected, T actual) where T : ISerializable<U>
    {
        string expectedJson = JsonUtility.ToJson(expected.Serialize());
        string actualJson = JsonUtility.ToJson(actual.Serialize());
        Assert.AreNotEqual(expectedJson, actualJson);
    }
}
