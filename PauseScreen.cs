using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScreen : MonoBehaviour
{

    public string levelSelect;
    public string mainMenu;

    private LevelManager theLevelManager;

    public GameObject thePauseScreen;

    private PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        //if(thePauseScreen == true)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        thePauseScreen.SetActive(false);

        //        Time.timeScale = 1f;
        //        thePlayer.canMove = true;
        //    }
        //}
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        thePauseScreen.SetActive(true);
        thePlayer.canMove = false;
        theLevelManager.levelMusic.Pause();
    }

    public void ResumeGame()
    {
        
        thePauseScreen.SetActive(false);

        Time.timeScale = 1f;
        thePlayer.canMove = true;
        theLevelManager.levelMusic.Play();
    }

    public void LevelSelect()
    {
        
        PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);
        PlayerPrefs.SetInt("CoinCount", theLevelManager.coinCount);

        Time.timeScale = 1f;

        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }
}
