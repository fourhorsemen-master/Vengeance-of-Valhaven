using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionSocketManager : MonoBehaviour
{
    private void Start()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        Random.InitState(currentRoomNode.TransitionModuleSeed);

        List<TransitionSocket> sockets = FindObjectsOfType<TransitionSocket>().ToList();
        sockets.SortById();

        if (!sockets.IsDistinctById())
        {
            Debug.LogError("Invalid transition socket ids, ensure all transition socket ids are unique.");
            return;
        }

        InstantiateModules(sockets, currentRoomNode);
    }

    private void InstantiateModules(List<TransitionSocket> sockets, RoomNode roomNode)
    {
        sockets.ForEach(socket =>
        {
            if (roomNode.ExitIdToChildLookup.ContainsKey(socket.AssociatedExitId))
            {
                InstantiateModule(socket, roomNode);
            }
        });
    }

    private void InstantiateModule(TransitionSocket socket, RoomNode roomNode)
    {
        TransitionModule prefab = GetAppropriatePrefab(roomNode, socket.AssociatedExitId);
        Instantiate(prefab, socket.transform).InstantiateIndicatedRoomTypes(
            roomNode.ExitIdToFurtherIndicatedRoomTypes[socket.AssociatedExitId]
        );
    }

    private TransitionModule GetAppropriatePrefab(RoomNode roomNode, int exitId)
    {
        if (roomNode.ExitIdToIndicatesNextRoomType[exitId])
        {
            RoomType nextRoomType = roomNode.ExitIdToChildLookup[exitId].RoomType;
            return RandomUtils.Choice(TransitionModuleLookup.Instance.TransitionModuleDictionary[nextRoomType].TransitionPrefabs);
        }

        return RandomUtils.Choice(TransitionModuleLookup.Instance.GenericModules);
    }
}