using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{

    static bool isPaused = false;
    GameObject gameOverScreen;

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
        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
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

    public void GameOver(Vector3 posDead)
    {
        gameOverScreen.SetActive(true);
        var gameOverCanvas = gameOverScreen.GetComponent<Canvas>().GetComponent<CanvasGroup>();
        DoFadeIn(gameOverCanvas);
    }

    IEnumerator DoFadeIn(CanvasGroup canvas)
    {
        while (canvas.alpha < 1)
        {
            canvas.alpha += 0.01f;
            yield return null;
        }
    }
}
