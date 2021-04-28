using System;
using UnityEngine;

[Serializable]
public class ExitData : IIdentifiable
{
    [SerializeField] private int id;
    [SerializeField] private Pole side;

    public int Id { get => id; set => id = value; }
    public Pole Side { get => side; set => side = value; }
}
