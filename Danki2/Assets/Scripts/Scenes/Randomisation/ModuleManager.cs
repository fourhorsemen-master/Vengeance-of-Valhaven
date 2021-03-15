using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    private float yRotation;

    private static readonly Dictionary<Pole, float> orientationToYRotation = new Dictionary<Pole, float>
    {
        [Pole.North] = 0,
        [Pole.East] = 90,
        [Pole.South] = 180,
        [Pole.West] = 270
    };

    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        Random.InitState(currentRoomSaveData.ModuleSeed);
        yRotation = orientationToYRotation[currentRoomSaveData.CameraOrientation];

        List<ModuleSocket> sockets = FindObjectsOfType<ModuleSocket>().ToList();
        sockets.SortById();

        if (!sockets.DistinctById())
        {
            Debug.LogError("Invalid socket ids, ensure all socket ids are unique.");
            return;
        }

        InstantiateModules(sockets);
    }

    private void InstantiateModules(List<ModuleSocket> sockets)
    {
        sockets.ForEach(socket =>
        {
            FaceTowardsCamera(socket);

            List<GameObject> modules = ModuleLookup.Instance.GetModulesWithMatchingTags(socket.SocketType, socket.Tags);

            if (modules.Count == 0)
            {
                Debug.LogError($"Socket found with tags that match no modules, ensure socket {socket.Id} has valid tags.");
                return;
            }

            GameObject module = RandomUtils.Choice(modules);
            Instantiate(module, socket.transform);
        });
    }

    private void FaceTowardsCamera(ModuleSocket socket)
    {
        Transform socketTransform = socket.transform;
        Vector3 socketRotation = socketTransform.eulerAngles;
        socketRotation.y = yRotation;
        socketTransform.eulerAngles = socketRotation;
    }
}
