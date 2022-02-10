using UnityEngine;

public class ShrineActivator : MonoBehaviour
{
    [SerializeField] private AbilityShrine abilityShrine = null;
    [SerializeField] private RuneShrine runeShrine = null;
    [SerializeField] private HealingShrine healingShrine = null;

    private void Start()
    {
        RoomType roomType = PersistenceManager.Instance.SaveData.CurrentRoomNode.RoomType;

        if (roomType == RoomType.Ability)
            abilityShrine.gameObject.SetActive(true);

        if (roomType == RoomType.Rune)
            runeShrine.gameObject.SetActive(true);

        if (roomType == RoomType.Healing)
            healingShrine.gameObject.SetActive(true);
    }
}
