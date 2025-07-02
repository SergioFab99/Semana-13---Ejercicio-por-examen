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
    public Button rankingButton;

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
        if (rankingButton != null)
        {
            rankingButton.onClick.AddListener(GoToRanking);
        }
    }

    public void RestartGame()
    {
        Debug.Log("Intentando cargar escena: " + GameManager.lastGameScene); // Para debug
        
        // Cargar la escena de juego desde la que viniste
        if (!string.IsNullOrEmpty(GameManager.lastGameScene))
        {
            SceneManager.LoadScene(GameManager.lastGameScene);
        }
        else
        {
            Debug.LogWarning("No se ha guardado la escena de juego anterior");
            // Fallback: intentar cargar una escena de juego por defecto
            SceneManager.LoadScene("GameScene"); // Cambia "GameScene" por el nombre de tu escena de juego principal
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Registro");
    }

    public void GoToRanking()
    {
        SceneManager.LoadScene("Ranking");
    }
}