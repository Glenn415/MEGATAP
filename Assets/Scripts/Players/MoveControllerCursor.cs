﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveControllerCursor : MonoBehaviour {
    [Header("Programmers - GameObjects/Scripts -----")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image controllerCursor;
    private CheckControllers checkControllers;
    private PauseMenu pause;

    [Header("Controller Values -----")]
    [SerializeField] private float cursorDelayHorizontal; //How long the script waits before moving cursor to next grid pos while stick is held down.
    [SerializeField] private float cursorDelayVertical;
    [SerializeField] private float freeRoamSpellSpeed;
    [SerializeField][Range(0, 1)] private float spellBarSpeedMultiplier;
    [SerializeField] private float cursorGrid;  //Size of the cursor grid - DIFFERENT from the size of the worldspace/trap grid. Should be fine at 23 but might need to be tweaked if we change UI.
    [Tooltip("Higher # = Lower Sensitivity")] [SerializeField] private float stickSensitivity; //Between 0-1 ; how far the player needs to push the stick for it to move the cursor
                                                                                               //Needed to set it higher because some controller sticks naturally move left/right a little.

    [Header("Audio-------------")]
    [SerializeField] private AudioClip spaceSelectionSFX;
    //private AudioSource audioSource;

    private bool p2Controller;
    private bool cursorHorizontalMove = true;
    private bool cursorVerticalMove = true;
    private InputManager inputManager;

    [HideInInspector]
    public bool MovingTraps = true;
    [HideInInspector]
    public SpellDirection SpellCastDirection = SpellDirection.Instant;

    private float screenWidth, screenHeight;

    private void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    void Start () {
        pause = gameManager.GetComponent<PauseMenu>();
        checkControllers = gameManager.GetComponent<CheckControllers>();
        //audioSource = GetComponent<AudioSource>();
        p2Controller = checkControllers.GetControllerTwoState();


        //Screen size
        CanvasScaler scaler = controllerCursor.GetComponentInParent<CanvasScaler>();
        float expectedAspectRatio = scaler.referenceResolution.x / scaler.referenceResolution.y;
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        screenWidth = (aspectRatio / expectedAspectRatio) * (scaler.referenceResolution.x / 2);
        screenHeight = scaler.referenceResolution.y / 2;
    }
	
    //Move cursor with grid
	void Update () {
        p2Controller = checkControllers.GetControllerTwoState();

        if (p2Controller && !pause.GameIsPaused && MovingTraps)
        {
            float horizontalInput, verticalInput;
            horizontalInput = inputManager.GetAxis(InputCommand.TopPlayerMoveHorizontal1) + inputManager.GetAxis(InputCommand.TopPlayerMoveHorizontal2);
            verticalInput = inputManager.GetAxis(InputCommand.TopPlayerMoveVertical1) + inputManager.GetAxis(InputCommand.TopPlayerMoveVertical2);

            Vector3 cursorPos = controllerCursor.GetComponent<RectTransform>().localPosition;

            if (horizontalInput > stickSensitivity && cursorHorizontalMove && cursorPos.x < screenWidth)
            {
                controllerCursor.GetComponent<RectTransform>().localPosition += new Vector3(cursorGrid, 0, 0);
                cursorHorizontalMove = false;
                StartCoroutine(EnableHorizontalCursorMove());

                //audioSource.PlayOneShot(spaceSelectionSFX);
            }
            else if (horizontalInput < -stickSensitivity && cursorHorizontalMove && cursorPos.x > -screenWidth)
            {
                controllerCursor.GetComponent<RectTransform>().localPosition -= new Vector3(cursorGrid, 0, 0);
                cursorHorizontalMove = false;
                StartCoroutine(EnableHorizontalCursorMove());
                //audioSource.PlayOneShot(spaceSelectionSFX);
            }
            else if (verticalInput > stickSensitivity && cursorVerticalMove && cursorPos.y < screenHeight)
            {
                controllerCursor.GetComponent<RectTransform>().localPosition += new Vector3(0, cursorGrid, 0);
                cursorVerticalMove = false;
                StartCoroutine(EnableVerticalCursorMove());
               // audioSource.PlayOneShot(spaceSelectionSFX);
            }
            else if (verticalInput < -stickSensitivity && cursorVerticalMove && cursorPos.y > 0)
            {
                controllerCursor.GetComponent<RectTransform>().localPosition -= new Vector3(0, cursorGrid, 0);
                cursorVerticalMove = false;
                StartCoroutine(EnableVerticalCursorMove());
                //audioSource.PlayOneShot(spaceSelectionSFX);
            }
        }
        else if (p2Controller && !pause.GameIsPaused && !MovingTraps)
        {
            float horizontalInput, verticalInput;
            horizontalInput = inputManager.GetAxis(InputCommand.TopPlayerMoveHorizontal1) + inputManager.GetAxis(InputCommand.TopPlayerMoveHorizontal2);
            verticalInput = inputManager.GetAxis(InputCommand.TopPlayerMoveVertical1) + inputManager.GetAxis(InputCommand.TopPlayerMoveVertical2);

            if (SpellCastDirection == SpellDirection.Instant)
            {
                Vector3 cursorPos = controllerCursor.GetComponent<RectTransform>().localPosition;

                if (verticalInput > stickSensitivity && cursorPos.y < -55)
                {
                    Vector3 pos = controllerCursor.transform.localPosition;
                    pos.z = 35;
                    
                    controllerCursor.transform.Translate(new Vector3(0f, verticalInput * freeRoamSpellSpeed, 0f));
                    
                }
                if (verticalInput < -stickSensitivity && cursorPos.y > -screenHeight)
                {
                    Vector3 pos = controllerCursor.transform.localPosition;
                    pos.z = 35;

                    controllerCursor.transform.Translate(new Vector3(0f, verticalInput * freeRoamSpellSpeed, 0f));

                }
                if (horizontalInput < -stickSensitivity)
                {
                    Vector3 pos = controllerCursor.transform.localPosition;
                    pos.z = 35;
                    if(controllerCursor.transform.localPosition.x > -screenWidth)
                    {
                        controllerCursor.transform.Translate(new Vector3(horizontalInput * freeRoamSpellSpeed, 0f, 0f));
                    }
                }
                if(horizontalInput > stickSensitivity)
                {
                    Vector3 pos = controllerCursor.transform.localPosition;
                    pos.z = 35;
                    if (controllerCursor.transform.localPosition.x < screenWidth)
                    {
                        controllerCursor.transform.Translate(new Vector3(horizontalInput * freeRoamSpellSpeed, 0f, 0f));
                    }
                }
            }
            else if(SpellCastDirection == SpellDirection.Right)
            {

                Vector3 cursorPos = controllerCursor.GetComponent<RectTransform>().localPosition;
                if (verticalInput > stickSensitivity && cursorVerticalMove && cursorPos.y < -55)
                {
                    controllerCursor.GetComponent<RectTransform>().localPosition += new Vector3(0, cursorGrid * spellBarSpeedMultiplier, 0);
                }
                else if (verticalInput < -stickSensitivity && cursorVerticalMove && cursorPos.y > -screenHeight)
                {
                    controllerCursor.GetComponent<RectTransform>().localPosition -= new Vector3(0, cursorGrid * spellBarSpeedMultiplier, 0);
                }
            }
            else if(SpellCastDirection == SpellDirection.Ceiling)
            {
                Vector3 cursorPos = controllerCursor.GetComponent<RectTransform>().localPosition;
                if (horizontalInput > stickSensitivity && cursorHorizontalMove && cursorPos.x < screenWidth)
                {
                    controllerCursor.GetComponent<RectTransform>().localPosition += new Vector3(cursorGrid * spellBarSpeedMultiplier, 0, 0);
                }
                else if (horizontalInput < -stickSensitivity && cursorHorizontalMove && cursorPos.x > -screenWidth)
                {
                    controllerCursor.GetComponent<RectTransform>().localPosition -= new Vector3(cursorGrid * spellBarSpeedMultiplier, 0, 0);
                }
            }
        }


    }

    IEnumerator EnableHorizontalCursorMove()
    {
        yield return new WaitForSeconds(cursorDelayHorizontal);
        cursorHorizontalMove = true;
    }

    IEnumerator EnableVerticalCursorMove()
    {
        yield return new WaitForSeconds(cursorDelayVertical);
        cursorVerticalMove = true;
    }
}
