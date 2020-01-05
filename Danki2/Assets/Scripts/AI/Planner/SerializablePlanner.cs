using System;
using UnityEngine;

[Serializable]
public class SerializablePlanner : ISerializationCallbackReceiver
{
    public Planner planner;

    [SerializeField]
    private SerializableType _serializablePlannerType;

    [SerializeField]
    private float[] _args;

    public SerializablePlanner()
    {
        planner = new NoOpPlanner();
        _serializablePlannerType = new SerializableType(planner.GetType());
        _args = new float[0];
    }

    public SerializablePlanner(Type plannerType, float[] args)
    {
        _serializablePlannerType = new SerializableType(plannerType);
        _args = args;

        InitializePlanner();
    }

    public void OnAfterDeserialize()
    {
        InitializePlanner();
    }

    private void InitializePlanner()
    {
        planner = (Planner)Activator.CreateInstance(_serializablePlannerType.Type);
        planner.Initilize(_args);
    }

    public void OnBeforeSerialize()
    {
    }
}
