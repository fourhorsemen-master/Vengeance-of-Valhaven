using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class TestUtils
{
    private static readonly Dictionary<Type, string> assetPathLookup = new Dictionary<Type, string>
    {
        [typeof(AbilityLookup)] = "Assets/Prefabs/Meta/GameplayManagers/AbilityLookup.prefab",
        [typeof(RarityLookup)] = "Assets/Prefabs/Meta/GameplayManagers/RarityLookup.prefab",
        [typeof(MapGenerationLookup)] = "Assets/Prefabs/Meta/MapGenerationLookup.prefab",
        [typeof(SceneLookup)] = "Assets/Prefabs/Meta/SceneLookup.prefab",
        [typeof(MapGenerator)] = "Assets/Prefabs/Meta/MapGenerator.prefab",
        [typeof(NewSaveGenerator)] = "Assets/Prefabs/Meta/NewSaveGenerator.prefab",
    };
    
    public static T InstantiatePrefab<T>() where T : Object
    {
        string assetPath = assetPathLookup[typeof(T)];
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
