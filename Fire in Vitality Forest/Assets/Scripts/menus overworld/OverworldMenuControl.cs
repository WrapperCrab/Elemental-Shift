using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverworldMenuControl : MenuControl
{
    #region Singleton

    public static OverworldMenuControl instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of OverworldMenuControl found!");
            return;
        }
        instance = this;
    }

    #endregion

    public void switchMenu(int buttonNumber)//!!!should only be activated when have control and did not switch control this frame
    {
        //update selectedButton
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }
        switch (buttonNumber)
        {
            case 1://save
                //ControlManager.instance.switchControl();
                break;
            case 2://inventory
                ControlManager.instance.switchControl(gameObject.transform.Find("InventoryMenu").GetComponent<Controllable>());
                break;
            case 3://skills
                //ControlManager.instance.switchControl();
                break;
            case 4://enepedia
                //ControlManager.instance.switchControl();
                break;
            case 5://settings
                ControlManager.instance.switchControl(gameObject.transform.Find("SettingsMenu").GetComponent<Controllable>());
                break;
            case 6://quit
                ControlManager.instance.switchControl(gameObject.transform.Find("QuitMenu").GetComponent<Controllable>());
                break;
            default:
                ControlManager.instance.switchControl(PlayerMovement.instance);
                break;
        } 
    }
}
