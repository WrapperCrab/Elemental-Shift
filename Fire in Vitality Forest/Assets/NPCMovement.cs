using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    public Rigidbody2D rigidBodyNpc;
    public float walkSpeed;
    public float runSpeed;

    bool isTalking;

    // Start is called before the first frame update
    void Start()
    {
        //start walking coroutine
     //   StartCoroutine(walking());
    }

    // Update is called once per frame
    void Update()
    {

    }

/*    IEnumerator walking()//I will use this if I ever want an npc to walk by themselves in their environment
    {
        //Choose random direction

        //walk briefly in that direction
    }*/
}
