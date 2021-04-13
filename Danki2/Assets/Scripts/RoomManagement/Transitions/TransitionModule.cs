using System.Collections.Generic;
using UnityEngine;

public class TransitionModule : MonoBehaviour
{
    [SerializeField] private List<IndicatorSocket> indicatorSockets = null;

    public void InstantiateIndicatedRoomTypes(List<RoomType> indicatedRoomTypes)
    {
        if (indicatedRoomTypes.Count > indicatorSockets.Count)
        {
            Debug.LogError("Tried to instantiate more room type indicators than were available on the transition module");
            return;
        }

        for (int i = 0; i < indicatedRoomTypes.Count; i++)
        {
            GameObject prefab = RandomUtils.Choice(
                TransitionModuleLookup
                    .Instance
                    .TransitionModuleDictionary[indicatedRoomTypes[i]]
                    .IndicatorPrefabs
            );
            Instantiate(prefab, indicatorSockets[i].transform);
        }
    }
}
