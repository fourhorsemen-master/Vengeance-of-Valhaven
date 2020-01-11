using System;
using UnityEngine;

[Serializable]
public class SerializableBehaviour : ISerializationCallbackReceiver
{
    public Behaviour behaviour;

    [SerializeField]
    private SerializableType _serializedType;

    [SerializeField]
    private float[] _serializedArgs;

    public SerializableBehaviour()
    {
        InitializeBehaviour(typeof(NoOpBehaviour), new float[0]);
    }

    public SerializableBehaviour(Type behaviourType, float[] args)
    {
        InitializeBehaviour(behaviourType, args);
    }

    public void OnAfterDeserialize()
    {
        InitializeBehaviour(_serializedType.Type, _serializedArgs);
    }

    public void OnBeforeSerialize()
    {
        _serializedArgs = behaviour.Args;
        _serializedType = new SerializableType(behaviour.GetType());
    }

    private void InitializeBehaviour(Type behaviourType, float[] args)
    {
        behaviour = (Behaviour)Activator.CreateInstance(behaviourType);
        behaviour.Args = args;
    }
}
