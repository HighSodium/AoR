using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class PlayerLoopDirector : MonoBehaviour
{

    // ------------- VARIABLES ----------------

    [Tooltip ("The original player prefab that will be cloned and replicated for each loop.")]
    public GameObject Original;

    [Tooltip("The prefab that will be used for the clones.")]
    public GameObject Clone;

    [Tooltip("The virtual cam that it being used in the scene")]
    public CinemachineVirtualCamera sceneCam;
   
    public Transform spawnLocReference;
    public bool overrideSpawn;
    public Vector2 spawnLocation;


    [HideInInspector] public float currentLoopTime;
    [HideInInspector] public Vector2 initPosition;
    [HideInInspector] public int currentLoop;
    [HideInInspector] public List<OrderedDictionary> RecordList = new List<OrderedDictionary>();
    [HideInInspector] public List<GameObject> CloneList = new List<GameObject>();

    // ------------- FUNCTIONS ----------------

    
        // Start is called before the first frame update
    void Start()
    {
        currentLoop = 0;
        CreateInitalPlayer();
            
    }

    void Update()
    {
        currentLoopTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();

            //---------------------------------------------------------
            // INSERT WAIT ANIMATION/SEQUENCE HERE INSTEAD OF TELEPORT
            //---------------------------------------------------------

            foreach (OrderedDictionary moves in RecordList){
                CreateClone(moves);
            }
        }
    }

    /// <summary>
    /// Creates clones of a player and inserts the recorded movesets into them
    /// </summary>
    private GameObject CreateClone(OrderedDictionary moveSet)
    {
        GameObject newClone = Instantiate(Clone, initPosition, Quaternion.identity);
        CloneList.Add(newClone);
        foreach(DictionaryEntry de in moveSet)
        {
            newClone.GetComponent<CloneMovementPlayback>().movementQueue.Add(de.Key, de.Value);      
        }
        //newClone.GetComponent<SpriteRenderer>().sortingLayerName = "Clones";
        //newClone.GetComponent<SpriteRenderer>().color = ???????

        return newClone;
    }

    /// <summary>
    /// Creates the Original player to be used and spawns them.
    /// </summary>
    private void CreateInitalPlayer()
    {
        // -> Get player initial position
        //      -Spawn clones from that initial position      

        if (overrideSpawn)
            initPosition = spawnLocation;
        else
            initPosition = spawnLocReference.position;


        Original = Instantiate(Original, initPosition, Quaternion.identity);
        sceneCam.Follow = Original.transform;

    }

    private void ResetPlayer()
    {
        DestroyClones();
        SavePlayerMovements(Original.GetComponent<InputRecorder>().playerMovements);
        currentLoopTime = 0;
        Original.transform.position = initPosition;
    }

    private void DestroyClones()
    {
        foreach(GameObject clone in CloneList)
        {
            Destroy(clone);
        }
    }

    private OrderedDictionary SavePlayerMovements(OrderedDictionary savedMoves)
    {
        if (savedMoves.Count == 0) return savedMoves;
        OrderedDictionary tempDict = new OrderedDictionary();
        foreach(DictionaryEntry de in savedMoves)
        {
            tempDict.Add(de.Key, de.Value);
        }

        if (savedMoves.Count > 0)
        {
            RecordList.Add(tempDict);
        }
        Original.GetComponent<InputRecorder>().playerMovements.Clear();
        return savedMoves;
    }
}



// Update is called once per frame