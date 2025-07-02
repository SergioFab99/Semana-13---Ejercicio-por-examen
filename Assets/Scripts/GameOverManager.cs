using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text usernameDisplay;
    public TMP_Text gameTimeDisplay;
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "¡Game Over!\nTu tiempo: " + GameManager.lastScoreSubmitted.ToString("F2") + "s";
        }

        if (usernameDisplay != null)
        {
            usernameDisplay.text = "Jugador: " + GameManager.currentUserName;
        }

        if (gameTimeDisplay != null)
        {
            gameTimeDisplay.text = "Tiempo de Supervivencia: " + GameManager.lastScoreSubmitted.ToString("F2") + "s";
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("FlappyEscena");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Registro");
    }
}
