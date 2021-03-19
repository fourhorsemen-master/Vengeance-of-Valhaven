using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionModuleManager : Singleton<TransitionModuleManager>
{
    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        Random.InitState(currentRoomSaveData.TransitionModuleSeed);

        List<TransitionSocket> sockets = FindObjectsOfType<TransitionSocket>().ToList();
        sockets.SortById();

        if (!sockets.DistinctById())
        {
            Debug.LogError("Invalid socket ids, ensure all socket ids are unique.");
            return;
        }

        InstantiateModules(
            sockets,
            currentRoomSaveData.PlayerSpawnerId,
            currentRoomSaveData.RoomTransitionerIdToNextRoomId.Keys.ToList()
        );
    }

    private void InstantiateModules(List<TransitionSocket> sockets, int playerSpawnerId, List<int> activeExitIds)
    {
        sockets.ForEach(socket =>
        {
            if (socket.HasAssociatedEntrance && playerSpawnerId == socket.AssociatedEntranceId) return;

            if (socket.HasAssociatedExit && activeExitIds.Contains(socket.AssociatedExitId))
            {
                InstantiateExit(socket);
            }
            else
            {
                InstantiateBlocker(socket);
            }
        });
    }

    private void InstantiateExit(TransitionSocket socket)
    {
        List<GameObject> modules = TransitionModuleLookup.Instance.GetExitModulesWithMatchingTags(socket.Tags);
        if (modules.Count == 0)
        {
            Debug.LogError($"Exit socket found with tags that match no modules, ensure socket {socket.Id} has valid tags.");
            return;
        }
        Instantiate(RandomUtils.Choice(modules), socket.transform);
    }

    private void InstantiateBlocker(TransitionSocket socket)
    {
        List<GameObject> modules = TransitionModuleLookup.Instance.GetBlockersWithMatchingTags(socket.Tags);
        if (modules.Count == 0)
        {
            Debug.LogError($"Blocker socket found with tags that match no modules, ensure socket {socket.Id} has valid tags.");
            return;
        }
        Instantiate(RandomUtils.Choice(modules), socket.transform);
    }
}
