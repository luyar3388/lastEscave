using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activeInventory = false;
    public MultiPlayer player;
    public GameObject lighter;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            lighter.SetActive(player.lighter);
        }

    }

}
