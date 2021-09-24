using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class TestUtils
{
    private static readonly Dictionary<Type, string> assetPathLookup = new Dictionary<Type, string>
    {
        [typeof(AbilityLookup2)] = "Assets/Prefabs/Meta/GameplayManagers/AbilityLookup2.prefab",
        [typeof(RarityLookup)] = "Assets/Prefabs/Meta/GameplayManagers/RarityLookup.prefab",
        [typeof(RuneLookup)] = "Assets/Prefabs/Meta/GameplayManagers/RuneLookup.prefab",
        [typeof(DevPersistenceManager)] = "Assets/Prefabs/Meta/GameplayManagers/DevManagers.prefab",
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
    
    public static void LoadEmptyScene() => SceneManager.LoadScene("EmptyScene");
}
