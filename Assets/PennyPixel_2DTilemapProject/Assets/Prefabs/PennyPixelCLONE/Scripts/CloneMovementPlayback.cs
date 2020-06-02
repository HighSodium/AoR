using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

// TODO: There is drift on X axis during jumps against walls.  Not sure why.
public class CloneMovementPlayback : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float moveGravity = 3;
    public float deadZone = 0.001f;
    [HideInInspector] public OrderedDictionary movementQueue = new OrderedDictionary();

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private GameObject director;
    private float velX, directorTime, currKey;
    private int left, right, dir, moveCount;
    private bool jumpUp, jumpDown;
    private string moveCommand;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveCount = 0;
        director = GameObject.Find("LoopDirector");

    }

    protected override void ComputeVelocity()
    {
        directorTime = director.GetComponent<PlayerLoopDirector>().currentLoopTime;

        //When detected that there are moves in queue split for 1 operation
        if(moveCount < movementQueue.Count)
        {

            // Likes to throw errors if buttons are pressed too fast
            try
            {
                currKey = (float)movementQueue.Cast<DictionaryEntry>().ElementAt(moveCount).Key;
            }
            catch
            {
            }
            if (directorTime >= currKey)
            {
                moveCommand = movementQueue[moveCount].ToString();
                IssueMoveCommand(moveCommand);
                moveCount++;
            }

        }
 
        Vector2 move = Vector2.zero;
        //move.x = Input.GetAxis("Horizontal");
        //Check to see if left or right is true and accelerate clone by player X gravity

        //THIS FUCKING AWFUL MESS emulates the acceleration of an InputAxis.  Not perfect.
        if (left == 1 || right == 1)
        {
            dir = Math.Sign(velX);
            velX = Mathf.Clamp(velX + ((right - left) * moveGravity * Time.deltaTime), -1, 1);

            if ((right == 1 && dir == -1) || (left == 1 && dir == 1))
                if(!(right == 1 && left == 1))
                    velX += moveGravity * Time.deltaTime * -dir;
        }
        else if(velX > deadZone || velX < -deadZone)
        {
            velX += moveGravity * Time.deltaTime * -Math.Sign(velX);
        }
        
        move.x = velX;
        
        if (jumpDown && grounded){
            velocity.y = jumpTakeOffSpeed;        
        }
        else if (jumpUp){          
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }          
        }
        jumpDown = jumpUp = false;

        if (move.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    private void IssueMoveCommand(string moveCommand)
    {
        switch (moveCommand)
        {
            case "LeftDown":
                left = 1;
                break;
            case "RightDown":
                right = 1;
                break;
            case "LeftUp":
                left = 0;
                break;
            case "RightUp":
                right = 0;
                break;
            case "JumpDown":
                jumpDown = true;
                break;
            case "JumpUp":
                jumpUp = true;
                break;
        }
    }
}

