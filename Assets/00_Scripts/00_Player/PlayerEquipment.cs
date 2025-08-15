using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public GameObject axe;
    public GameObject pickaxe;
    public GameObject sword;

    private void Start()
    {
        EquipNew(ToolType.Axe);
    }

    public void EquipNew(ToolType toolType)
    {
        UnEquip();
        switch (toolType)
        {
            case ToolType.Axe:
                axe.SetActive(true);
                break;
            case ToolType.Pickaxe:
                pickaxe.SetActive(true);
                break;
            case ToolType.Sword:
                sword.SetActive(true);
                break;
        }
    }

    public void UnEquip()
    {
        axe.SetActive(false);
        pickaxe.SetActive(false);
        sword.SetActive(false);
    }
}
