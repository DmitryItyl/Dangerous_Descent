using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{

    static bool isPaused = false;
    GameObject gameOverScreen;
    GameObject door;

    public void StartGame()
    {
        LoadLevel(1);
    }

    public void LoadMainMenu()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1f;
        door = GameObject.Find("Door");
    }

    public void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 1 || currentSceneIndex == 2)
            LoadLevel(currentSceneIndex + 1);
        else if (currentSceneIndex == 3)
            LoadMainMenu();
    }

    public void CurrentLevelClear()
    {
        //door.GetComponent<LevelFinish>().StageClear();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void SetPaused(bool _isPaused)
    {
        isPaused = _isPaused;
    }
}
