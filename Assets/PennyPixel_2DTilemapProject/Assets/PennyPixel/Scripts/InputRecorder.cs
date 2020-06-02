using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

//TODO: Needs alternate tech to log inputs on button press.

public class InputRecorder : MonoBehaviour
{
    
    public bool playerCanControl;

    GameObject director;
    string input;
    float curTime;

    [HideInInspector] public OrderedDictionary playerMovements = new OrderedDictionary();
    [HideInInspector] public bool resetMoves;


    // Start is called before the first frame update
    void Start()
    {
        playerCanControl = true;
        resetMoves = false;
        director = GameObject.Find("LoopDirector");

        
    }

    // Update is called once per frame
    void Update()
    {
        curTime = director.GetComponent<PlayerLoopDirector>().currentLoopTime;
        if (playerCanControl)
        {
            //input = CheckInputs();
            LogInputs();
            if(input != null)
            {
                print(input);              
                playerMovements.Add(curTime, input);
            }
        }
    }
    // END MAIN

    // Adds the inputs to a dictionary NEEDS OTHER METHOD BESIDES DIRECT KEYS!!!
    private void LogInputs()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddMove("RightDown");
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddMove("LeftDown");
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            AddMove("RightUp");
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            AddMove("LeftUp");
        }


        if (Input.GetButtonDown("Jump"))
        {
            AddMove("JumpDown");
        }

        if (Input.GetButtonUp("Jump"))
        {
            AddMove("JumpUp");
        }
    }

    private void AddMove(string moveName)
    {
        print(moveName);
        if(playerMovements.Contains(curTime))
            playerMovements.Add(curTime + 0.001, moveName);
        else
            playerMovements.Add(curTime, moveName);
    }

    // DEPRECIATED Not as accurate as LogInputs().
    public string CheckInputs()
    {

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return "RightDown";
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return "LeftDown";


        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            return "RightUp";
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            return "LeftUp";


        if (Input.GetButtonDown("Jump"))
        {
            return "JumpDown";
        }

        if (Input.GetButtonUp("Jump"))
        {
            return "JumpUp";
        }

        return null;
    }
}
