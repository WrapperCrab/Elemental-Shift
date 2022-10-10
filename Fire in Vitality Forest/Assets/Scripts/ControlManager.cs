using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    #region Singleton

    public static ControlManager instance;
    void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of ControlManager found!");
            Destroy(gameObject);//destroys the extra (this)
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    //this is to be used to keep player control in only one place at a time. 
    //If a player switches to a different area of control in a frame, we have a bool here
    //which keeps the player's input from being counted again in the new area

    bool switchedThisFrame = false;
    public Controllable obWithControl;

    void Start()
    {
        switchedThisFrame = false;
        //get the scene we are in
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Overworld")
        {
            obWithControl = PlayerMovement.instance;
        }
        else if (sceneName == "Battle")
        {
            obWithControl = BattleSystem.instance;
        }
        else
        {
            Debug.Log("No obWithControl. Shit's gone south");
        }
    }

    void LateUpdate()
    {
        switchedThisFrame = false;//made false for the next frame
    }

    public bool getSwitched()
    {
        return switchedThisFrame;
    }

    public void switchControl(Controllable gainControl)
    {
        //changeAble and changeActive are usually the same for battle menus
        int oldDepth = obWithControl.getMenuDepth();
        int newDepth = gainControl.getMenuDepth();

        if (oldDepth < newDepth)
        {
            //disable old menu
            obWithControl.changeAble();
            //activate new menu
            gainControl.changeActive();

        }
        else if (oldDepth > newDepth)
        {
            //deactivate old menu
            obWithControl.changeActive();
            //enable new menu
            gainControl.changeAble();
        }
        else
        {
            //deactivate old menu
            obWithControl.changeActive();
            //activate new menu
            gainControl.changeActive();
        }
        obWithControl.switchControl();
        gainControl.switchControl();
        obWithControl = gainControl;
        switchedThisFrame = true;
    }
}
