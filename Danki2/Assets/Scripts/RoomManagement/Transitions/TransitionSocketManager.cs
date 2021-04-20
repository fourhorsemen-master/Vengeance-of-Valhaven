using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionSocketManager : MonoBehaviour
{
    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        Random.InitState(currentRoomSaveData.TransitionModuleSeed);

        List<TransitionSocket> sockets = FindObjectsOfType<TransitionSocket>().ToList();
        sockets.SortById();

        if (!sockets.DistinctById())
        {
            Debug.LogError("Invalid transition socket ids, ensure all transition socket ids are unique.");
            return;
        }

        InstantiateModules(sockets, currentRoomSaveData);
    }

    private void InstantiateModules(List<TransitionSocket> sockets, RoomSaveData roomSaveData)
    {
        sockets.ForEach(socket =>
        {
            if (roomSaveData.RoomTransitionerIdToTransitionData.TryGetValue(socket.AssociatedExitId, out TransitionData transitionData))
            {
                InstantiateModule(socket, transitionData);
            }
        });
    }

    private void InstantiateModule(TransitionSocket socket, TransitionData transitionData)
    {
        TransitionModule prefab = GetAppropriatePrefab(transitionData);
        Instantiate(prefab, socket.transform).InstantiateIndicatedRoomTypes(transitionData.FurtherIndicatedRoomTypes);
    }

    private TransitionModule GetAppropriatePrefab(TransitionData transitionData)
    {
        if (transitionData.IndicatesNextRoomType)
        {
            RoomType nextRoomType = PersistenceManager.Instance.SaveData.RoomSaveDataLookup[transitionData.NextRoomId].RoomType;
            return RandomUtils.Choice(TransitionModuleLookup.Instance.TransitionModuleDictionary[nextRoomType].TransitionPrefabs);
        }

        return RandomUtils.Choice(TransitionModuleLookup.Instance.GenericModules);
    }
}