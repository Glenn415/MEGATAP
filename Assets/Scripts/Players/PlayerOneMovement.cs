﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOneMovement : MonoBehaviour {
    //movement vars serialized for designers
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    //other movement vars
    private Vector3 movementVector;
    private bool crouching;
    private bool grounded;
    private bool jumping;

    //Control if player can have input
    private bool move;

    private float speed; //Change this when crouching, etc.; set it back to moveSpeed when done
    private float jumpH; // change this when in sap etc.; set it back to jumpHeight when done

    //camera
    [SerializeField] private CameraOneRotator cam;
    private int camOneState = 1;

    [SerializeField] private GameObject gameManager;

    private float inputAxis; //used to get input axis from controller/keyboard

	private Rigidbody rb;


    private CheckControllers checkControllers;
    private Animator animator;

    void Start() {
		rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        checkControllers = gameManager.GetComponent<CheckControllers>();

        speed = moveSpeed;
        jumpH = jumpHeight;

        move = true;
    }

    private void Update()
    {
        camOneState = cam.GetState();

        if (move == true)
        {
            inputAxis = checkControllers.GetInputAxis();

            //jumping
            if (Input.GetButtonDown("Jump_Joy_1") && grounded)
            {
                jumping = true;
            }

            //crouch
            if (Input.GetButton("Crouch_Joy_1") && grounded)
            {
                crouching = true;
            }
            if (Input.GetButtonUp("Crouch_Joy_1") && grounded)
            {
                crouching = false;
            }
            // Animation parameters update
            animator.SetBool("Jumping", jumping);
            animator.SetBool("Running", move);
        }

        switch (camOneState)
        {
            case 1:
                movementVector = new Vector3(inputAxis * speed * Time.deltaTime, rb.velocity.y, 0);
                //Debug.Log(movementVector);
                animator.SetFloat("Velocity", rb.velocity.x);
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                break;
            case 2:
                movementVector = new Vector3(0, rb.velocity.y, inputAxis * speed * Time.deltaTime);
                animator.SetFloat("Velocity", rb.velocity.z);
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
                break;
            case 3:
                movementVector = new Vector3(-inputAxis * speed * Time.deltaTime, rb.velocity.y, 0);
                animator.SetFloat("Velocity", -rb.velocity.x);
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                break;
            case 4:
                movementVector = new Vector3(0, rb.velocity.y, -inputAxis * speed * Time.deltaTime);
                animator.SetFloat("Velocity", -rb.velocity.z);
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
                break;
        }
    }

    private void FixedUpdate()
    {
        if(jumping)
        {
            movementVector = new Vector3(movementVector.x, jumpH, movementVector.z);
            jumping = false;
        }
        else if(crouching)
        {
            //TODO
        }
        if (move == false)
        {
            movementVector = new Vector3(0, movementVector.y, 0);
        }
        rb.velocity = movementVector;
    }
    

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            grounded = true;
        }
        else if (Physics.Raycast(transform.position, -transform.right, 1))
        {
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            grounded = false;
        }
    }

    /////////////////////////////////////////////
    // GETTERS AND SETTERS                     //
    /////////////////////////////////////////////
    public int GetState()
    {
        return camOneState;
    }

    public float GetJumpHeight()
    {
        return jumpH;
    }

    public void SetJumpHeight(float j)
    {
        jumpH = j;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public float GetConstantSpeed()
    {
        return moveSpeed;
    }

    public float GetConstantJumpHeight()
    {
        return jumpHeight;
    }

    public void SetMove(bool m)
    {
        move = m;
    }
}
