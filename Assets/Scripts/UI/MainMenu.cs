﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	[SerializeField] private GameObject popup;
    [SerializeField] private EventSystem es;

    [SerializeField] private GameObject[] menuButtons;

    [SerializeField] private GameObject[] yesNoButtons;

    private CheckControllers checkControllers;

    private GameObject inputManager;

    private void Awake()
    {
        inputManager = GameObject.Find("InputManager");
        //int musicPlayerCount = 0;
        //while(GameObject.Find("MusicPlayer") != null)
        //{
        //    musicPlayerCount++;
        //}
        //Debug.Log(musicPlayerCount);
    }

    void Start ()
    {
        //Set "Quit" popup inactive
		popup.gameObject.SetActive(false);

        //Detect controller connection & if connected, set selected button
        checkControllers = GetComponent<CheckControllers>();
        if(checkControllers.GetControllerOneState() || checkControllers.GetControllerTwoState())
        {
            es.SetSelectedGameObject(menuButtons[0].gameObject);
        }
	}

    private void Update()
    {
        if (checkControllers.GetControllerOneState() || checkControllers.GetControllerTwoState() && es.currentSelectedGameObject == null)
        {
            es.SetSelectedGameObject(menuButtons[0].gameObject);
        }
    }

    public void OnClickPlay()
    {
        CheckControllers cc = inputManager.GetComponent<CheckControllers>();

        if(cc.GetControllerOneState() && cc.GetControllerTwoState())
        {
            SceneManager.LoadScene("CharacterSelect");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
    
    public void QuitGame()
    {
		popup.gameObject.SetActive(true);

        //Handle controllers for Quit popup
        if (checkControllers.GetControllerOneState())
        {
            es.SetSelectedGameObject(yesNoButtons[0]);
            for(int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    
    public void YesButton()
    {
    	Debug.Log("Quit");
    	Application.Quit();
    }
    
    public void NoButton()
    {
    	popup.gameObject.SetActive(false);

        //Handle controllers again for regular menu
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].GetComponent<Button>().interactable = true;
        }
        es.SetSelectedGameObject(menuButtons[0]);
    }	
}
