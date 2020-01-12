using System;
using UnityEngine;

[Serializable]
public class SerializablePlanner : ISerializationCallbackReceiver
{
    public Planner planner;

    [SerializeField]
    private SerializableType _serializedType;

    [SerializeField]
    private float[] _serializedArgs;

    public SerializablePlanner()
    {
        InitializePlanner(typeof(NoOpPlanner), new float[0]);
    }

    public SerializablePlanner(Type plannerType, float[] args)
    {
        InitializePlanner(plannerType, args);
    }

    public void OnAfterDeserialize()
    {
        InitializePlanner(_serializedType.Type, _serializedArgs);
    }

    public void OnBeforeSerialize()
    {
        _serializedType = new SerializableType(planner.GetType());
        _serializedArgs = planner.Args;
    }

    private void InitializePlanner(Type plannerType, float[] args)
    {
        planner = (Planner)Activator.CreateInstance(plannerType);
        planner.Args = args;
    }
}
