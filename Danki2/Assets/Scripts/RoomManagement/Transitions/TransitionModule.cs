using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionModule : MonoBehaviour
{
    [SerializeField] private List<IndicatorSocket> indicatorSockets = null;

    public void InstantiateIndicatedRoomTypes(List<RoomType> indicatedRoomTypes)
    {
        List<RoomType> distinctIndicatedRoomTypes = indicatedRoomTypes.Distinct().ToList();

        if (distinctIndicatedRoomTypes.Count > indicatorSockets.Count)
        {
            Debug.LogError("Tried to instantiate more room type indicators than were available on the transition module");
            return;
        }

        for (int i = 0; i < distinctIndicatedRoomTypes.Count; i++)
        {
            GameObject prefab = RandomUtils.Choice(
                TransitionModuleLookup
                    .Instance
                    .TransitionModuleDictionary[distinctIndicatedRoomTypes[i]]
                    .IndicatorPrefabs
            );
            Instantiate(prefab, indicatorSockets[i].transform);
        }
    }
}
