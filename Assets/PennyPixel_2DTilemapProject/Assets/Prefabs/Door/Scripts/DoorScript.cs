using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{

    public List<GameObject> linkedButtons;
    private Animator animator;
    private bool isActive;

    // Start is called before the first frame update
    void Awake()
    {
        if(linkedButtons.Count == 0)
        {
            Debug.LogError(gameObject.name + " HAS NO LINKED BUTTONS!");
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isActive = CheckButtons(linkedButtons);
        animator.SetBool("isActive", isActive);
        GetComponent<BoxCollider2D>().isTrigger = isActive;
        //GetComponent<CompositeCollider2D>().isTrigger = isActive;


    }

    private bool CheckButtons(List<GameObject> buttons)
    {
        bool check = true;
        foreach(GameObject button in buttons)
        {
            if(button.GetComponent<ButtonScript>().isActive == false)
            {
                check = false;
                break;
            }
        }
        return check;
    }
}
