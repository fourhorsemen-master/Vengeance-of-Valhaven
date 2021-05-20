using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransitionModuleData
{
    [SerializeField] private List<TransitionModule> transitionPrefabs = new List<TransitionModule>();
    [SerializeField] private List<GameObject> indicatorPrefabs = new List<GameObject>();

    public List<TransitionModule> TransitionPrefabs { get => transitionPrefabs; set => transitionPrefabs = value; }
    public List<GameObject> IndicatorPrefabs { get => indicatorPrefabs; set => indicatorPrefabs = value; }
}

[Serializable]
public class TransitionModuleDictionary : SerializableEnumDictionary<RoomType, TransitionModuleData>
{
    public TransitionModuleDictionary(TransitionModuleData defaultValue) : base(defaultValue) {}
    public TransitionModuleDictionary(Func<TransitionModuleData> defaultValueProvider) : base(defaultValueProvider) {}
}

public class TransitionModuleLookup : Singleton<TransitionModuleLookup>
{
    [SerializeField]
    private TransitionModule emptyModule = null;
    public TransitionModule EmptyModule { get => emptyModule; set => emptyModule = value; }

    [SerializeField]
    private TransitionModuleDictionary transitionModuleDictionary = new TransitionModuleDictionary(() => new TransitionModuleData());
    public TransitionModuleDictionary TransitionModuleDictionary => transitionModuleDictionary;

}