using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.P;
    public bool isPaused = false;
    public bool isEndGame = false;

    [SerializeField] private GameObject hudCanvas = null;
    [SerializeField] private GameObject endGameCanvas = null;
    [SerializeField] private GameObject pauseCanvas = null;
    [SerializeField] private GameObject scoreBoardHolder = null;

    private PlayerController playerController;
    private PlayerStats playerStats;

    // Is called at the start of the game
    private void Start()
    {
        GetReferences();
        SetActiveHud(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (!isPaused)
            {
                SetActivePause(true);
                SetActiveScoreboard(false); // Disable the scoreboard when pause is activated
            }
            else
            {
                SetActivePause(false);
            }
        }

        if(playerStats.isDead)
        {
            SetActiveEndGame(true);
        }
    }

    public void SetActiveScoreboard(bool state)
    {
        scoreBoardHolder.SetActive(state);
    }

    public void SetActiveHud(bool state)
    {
        hudCanvas.SetActive(state);
        endGameCanvas.SetActive(!state);
        pauseCanvas.SetActive(!state && !playerStats.isDead);
    }

    public void SetActiveEndGame(bool state)
    {
        endGameCanvas.SetActive(state);
        hudCanvas.SetActive(!state);
        pauseCanvas.SetActive(!state && !playerStats.isDead);

        isEndGame = state;

        if (state)
        {
            playerController.UnlockCursor();
        }
        else
        {
            playerController.LockCursor();
        }
    }

    public void SetActivePause(bool state)
    {
        pauseCanvas.SetActive(state);
        hudCanvas.SetActive(!state && !playerStats.isDead);
        endGameCanvas.SetActive(false);

        Time.timeScale = state ? 0 : 1;
        isPaused = state;

        if (state)
        {
            playerController.UnlockCursor();
        }
        else
        {
            playerController.LockCursor();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GetReferences()
    {
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
    }
}