using System;
using UnityEngine;

[Serializable]
public class SerializableRuneRoomSaveData
{
    [SerializeField] private int seed = 0;
    [SerializeField] private bool runesViewed = false;
    [SerializeField] private bool runeSelected = false;

    public int Seed { get => seed; set => seed = value; }
    public bool RunesViewed { get => runesViewed; set => runesViewed = value; }
    public bool RuneSelected { get => runeSelected; set => runeSelected = value; }

    public RuneRoomSaveData Deserialize()
    {
        return new RuneRoomSaveData
        {
            Seed = Seed,
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
