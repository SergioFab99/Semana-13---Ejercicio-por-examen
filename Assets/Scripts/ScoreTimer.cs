using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.SceneManagement; // Necesario si LoadNextScene fuera a cargar una escena directamente por índice, pero ahora GameManager lo hace

public class ScoreTimer : MonoBehaviour
{
    // Asigna este TextMeshProUGUI desde el Inspector para mostrar el tiempo en pantalla
    public TextMeshProUGUI scoreText;

    // Variables internas del contador de tiempo
    private float timer = 0f;
    private bool isAlive = true; // Indica si el jugador está vivo y el tiempo debe correr

    // Método Update se llama una vez por frame
    void Update()
    {
        // Si el jugador está vivo, incrementa el contador de tiempo
        if (isAlive)
        {
            timer += Time.deltaTime; // Time.deltaTime es el tiempo que ha pasado desde el último frame
            // Actualiza el texto en la UI para mostrar el tiempo actual
            // ToString("F2") formatea el número a dos decimales
            if (scoreText != null)
            {
                scoreText.text = "Tiempo: " + timer.ToString("F2") + "s";
            }
        }
    }

    // Este método debe ser llamado desde la lógica de tu pájaro (ej. BirdController)
    // cuando el pájaro colisiona o "muere"
    public void PlayerDied()
    {
        // Si el jugador ya no está vivo, ignorar llamadas repetidas
        if (!isAlive) return;

        isAlive = false; // Detener el contador de tiempo

        // ======================================================================================
        // ¡¡CAMBIO CRÍTICO AQUÍ!!
        // Ya no cargamos la escena directamente desde aquí.
        // En su lugar, notificamos al GameManager que el jugador ha muerto y le pasamos el tiempo.
        // El GameManager se encargará de guardar el puntaje, y luego de cargar la escena de Game Over.
        // ======================================================================================

        // Asegúrate de que GameManager existe en tu escena y llama a su método PlayerDied()
        // GameManager.currentPlayedLevelId ya debe estar seteado cuando cargas la escena de juego.
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            // Pasa el tiempo de supervivencia actual al GameManager
            // El GameManager.PlayerDied() internamente usa GameManager.lastScoreSubmitted
            // y envía el puntaje a la base de datos.
            gameManager.PlayerDied();
        }
        else
        {
            Debug.LogError("ScoreTimer: No se encontró un GameManager en la escena. El puntaje no se registrará ni se cargará la escena de Game Over.");
            // Si no hay GameManager, y como última opción de fallback, podrías cargar la escena de Game Over
            // directamente, pero la puntuación no se guardaría en la DB.
            // SceneManager.LoadScene("NombreDeTuEscenaDeGameOver"); // Fallback
        }
    }

    // Este método ya no es necesario si GameManager maneja la carga de la siguiente escena.
    // private void LoadNextScene()
    // {
    //     int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //     SceneManager.LoadScene(currentSceneIndex + 1);
    // }

    // Opcional: Método para reiniciar el contador si el juego se reinicia sin recargar la escena
    public void ResetTimer()
    {
        timer = 0f;
        isAlive = true;
        if (scoreText != null)
        {
            scoreText.text = "Tiempo: 0.00s";
        }
    }
}
