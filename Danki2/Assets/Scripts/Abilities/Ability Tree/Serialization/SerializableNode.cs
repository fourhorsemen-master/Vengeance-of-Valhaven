using System;
using UnityEngine;

[Serializable]
public class SerializableNode
{
    [SerializeField] private AbilityReference abilityReference;
    [SerializeField] private int id;
    [SerializeField] private int leftChildId;
    [SerializeField] private int rightChildId;
    
    public AbilityReference AbilityReference => abilityReference;
    public int Id => id;
    public int LeftChildId => leftChildId;
    public int RightChildId => rightChildId;
    
    public SerializableNode(AbilityReference abilityReference, int id, int leftChildId, int rightChildId)
    {
        this.abilityReference = abilityReference;
        this.id = id;
        this.leftChildId = leftChildId;
        this.rightChildId = rightChildId;
    }
}
