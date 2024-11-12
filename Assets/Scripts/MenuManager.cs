using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject PauseMenuUI;
    // public bool isPaused {get; private set;}
     void Awake()
    {
        // Ensure only one instance of MenuManager exists
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);  // Optional: persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }
    private void Start()
    {

    }

    private void Update()
    {
        // if (PlayerInputManager.Instance.MenuOpened)
        // {
        //     Debug.Log("pause called");
        // }
        //     if (isPaused)
        //     {
        //         Unpause();
        //     }
        //     else 
        //     {
        //         Pause();
        //     }
    }

     public void ShowPauseMenu()
    {
        PauseMenuUI.SetActive(true);
    }

    public void HidePauseMenu()
    {
        PauseMenuUI.SetActive(false);
    }

}
