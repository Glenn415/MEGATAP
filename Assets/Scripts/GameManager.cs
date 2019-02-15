﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
	public static bool GameIsPaused = false;
	private PauseMenu pause;
    [SerializeField] private GameObject avoidVinesText;

    //Game Over Status
    [SerializeField] private PlayerOneLose lost;
    private bool lose = false;


    private string[] joysticks;

    private bool controllerOne;
    private bool controllerTwo;


    private void Awake()
    {
        joysticks = Input.GetJoystickNames();
        CheckControllers();
        SceneManager.LoadScene("Tower1_Platforms", LoadSceneMode.Additive);
        Debug.Log("Player 1 controller: " + controllerOne);
        Debug.Log("Player 2 controller: " + controllerTwo);
    }

    private void Update ()
    {
        //Game Over from timer
        lose = lost.GameOver();
        if(lose == true)
        {
            SceneManager.LoadScene("GameOver");
        }

        CheckControllers();

        if(Time.time > 35f && Time.time <= 38f)
        {
            avoidVinesText.SetActive(true);
        }

	}

    private void CheckControllers()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (!string.IsNullOrEmpty(joysticks[i]))
                {
                    //Debug.Log("Controller " + i + " is connected using: " + joysticks[i]);
                    if (i == 0)
                    {
                        controllerOne = true;
                    }
                    if (i == 1)
                    {
                        controllerTwo = true;
                    }
                }
                else
                {
                    //Debug.Log("Controller " + i + " is disconnected.");
                    if (i == 0)
                    {
                        controllerOne = false;
                    }
                    if(i == 1)
                    {
                        controllerTwo = false;
                    }
                }
            }
        }
        else
        {
            controllerOne = false;
            controllerTwo = false;
        }
    }

    public bool GetControllerTwoState()
    {
        return controllerTwo;
    }

    //Get InputAxis based on whether player1 is using controller or keyboard
    public float GetInputAxis()
    {
        if (controllerOne)
        {
            if(Mathf.Abs(Input.GetAxis("Horizontal_Joy_1")) > 0.4f)
            {
                return Input.GetAxis("Horizontal_Joy_1");
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return Input.GetAxisRaw("Horizontal_Keyboard");
        }
    }
}
