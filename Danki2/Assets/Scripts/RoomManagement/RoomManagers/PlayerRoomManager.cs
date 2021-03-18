using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles spawning the player in the rooms that require it. If the room does not require the player then this class
/// does nothing.
/// </summary>
public class PlayerRoomManager : Singleton<PlayerRoomManager>
{
    private static readonly ISet<RoomType> playerRoomTypes = new HashSet<RoomType>
    {
        RoomType.Combat,
        RoomType.Boss,
        RoomType.Shop
    };
    
    private void Start()
    {
        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;

        if (!playerRoomTypes.Contains(roomSaveData.RoomType)) return;

        Dictionary<int, PlayerSpawner> spawnerLookup = FindObjectsOfType<PlayerSpawner>().ToDictionary(s => s.Id);

        spawnerLookup[roomSaveData.PlayerSpawnerId].Spawn();
    }
}
