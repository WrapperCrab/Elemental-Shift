using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDictionary : MonoBehaviour
{
    #region Singleton

    public static BattleDictionary instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleDictionary found!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
