﻿using System;
using UnityEngine;

[Serializable]
public class SerializableRuneRoomSaveData
{
    [SerializeField] private bool runesViewed = false;
    [SerializeField] private bool runeSelected = false;

    public bool RunesViewed { get => runesViewed; set => runesViewed = value; }
    public bool RuneSelected { get => runeSelected; set => runeSelected = value; }

    public RuneRoomSaveData Deserialize()
    {
        return new RuneRoomSaveData
        {
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
