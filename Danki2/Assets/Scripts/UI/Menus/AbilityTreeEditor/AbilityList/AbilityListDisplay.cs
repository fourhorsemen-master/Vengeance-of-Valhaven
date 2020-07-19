﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityListDisplay : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private AbilityListingPanel abilityListingPanelPrefab = null;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup = null;

    void Start()
    {
        player = RoomManager.Instance.Player;

        AbilityTreeEditorMenu.Instance.AbilityDragStopSubject.Subscribe(_ => PopulateAbilityList());
    }

    private void OnEnable()
    {
        // This is called before Start, at which point the player is still null
        if (player != null) PopulateAbilityList();
    }

    private void PopulateAbilityList()
    {
        //destroy all current panels
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (KeyValuePair<AbilityReference, int> item in player.AbilityTree.Inventory)
        {
            if (item.Value > 0)
            {
                AbilityListingPanel abilityListingPanel = Instantiate(abilityListingPanelPrefab, Vector3.zero, Quaternion.identity, transform);
                abilityListingPanel.Initialise(item.Key, item.Value);
            }
        }

        // PreferredHeight isn't up to date until next frame, so we set the content height then.
        this.NextFrame(
            () => rectTransform.SetHeight(verticalLayoutGroup.preferredHeight)
        );
    }
}
