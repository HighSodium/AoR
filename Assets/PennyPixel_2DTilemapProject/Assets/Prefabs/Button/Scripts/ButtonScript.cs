using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool isActive = false;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //isActive = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isActive", isActive);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (layer == 8 || layer == 9)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (layer == 8 || layer == 9)
        {
            isActive = false;
        }
    }
}
