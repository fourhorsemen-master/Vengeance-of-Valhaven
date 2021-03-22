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
        
        InstantiateModules(sockets, currentRoomSaveData.RoomTransitionerIdToNextRoomId.Keys.ToList());
    }

    private void InstantiateModules(List<TransitionSocket> sockets, List<int> activeExitIds)
    {
        sockets.ForEach(socket =>
        {
            if (activeExitIds.Contains(socket.AssociatedExitId))
            {
                Instantiate(RandomUtils.Choice(TransitionModuleLookup.Instance.Modules), socket.transform);
            }
        });
    }
}
