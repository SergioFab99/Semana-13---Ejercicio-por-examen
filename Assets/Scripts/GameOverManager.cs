using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.UI; // Necesario para Button
using UnityEngine.SceneManagement; // Para cargar otras escenas

public class GameOverManager : MonoBehaviour
{
    // =====================================================================
    // ASIGNABLES DESDE EL INSPECTOR (en tu escena de Game Over)
    // =====================================================================
    public TMP_Text finalScoreText;     // TextMeshPro para mostrar el puntaje final grande (ya existía)
    public TMP_Text usernameDisplay;    // ¡¡NUEVO!! TextMeshPro para mostrar el nombre del usuario
    public TMP_Text gameTimeDisplay;    // ¡¡NUEVO!! TextMeshPro para mostrar el tiempo del juego (redundante con finalScoreText, pero añadido según petición)
    public Button restartButton;        // Botón para reiniciar la partida
    public Button mainMenuButton;       // Botón para volver al menú principal/selección de niveles

    // =====================================================================
    // MÉTODOS DE CICLO DE VIDA DE UNITY
    // =====================================================================
    void Start()
    {
        // Mostrar la puntuación obtenida del GameManager
        if (finalScoreText != null)
        {
            finalScoreText.text = "¡Game Over!\nTu tiempo: " + GameManager.lastScoreSubmitted.ToString("F2") + "s";
        }

        // Mostrar el nombre del usuario (desde GameManager.currentUserName)
        if (usernameDisplay != null)
        {
            usernameDisplay.text = "Jugador: " + GameManager.currentUserName;
        }

        // Mostrar el tiempo del juego (desde GameManager.lastScoreSubmitted)
        // Nota: finalScoreText ya muestra esto, este es un campo adicional según tu petición.
        if (gameTimeDisplay != null)
        {
            gameTimeDisplay.text = "Tiempo de Supervivencia: " + GameManager.lastScoreSubmitted.ToString("F2") + "s";
        }

        // Asignar listeners a los botones
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
    }

    // =====================================================================
    // FUNCIONALIDADES DE BOTONES (en la escena de Game Over)
    // =====================================================================
    public void RestartGame()
    {
        // Vuelve a cargar la escena de juego (FlappyEscena)
        // Asegúrate de que "FlappyEscena" sea el nombre exacto de tu escena de juego.
        SceneManager.LoadScene("FlappyEscena");
    }

    public void GoToMainMenu()
    {
        // Vuelve a la escena de login/registro (asumiendo que "Registro" es tu escena principal de menú)
        // Asegúrate de que "Registro" sea el nombre exacto de tu escena de menú principal.
        SceneManager.LoadScene("Registro");
    }
}
