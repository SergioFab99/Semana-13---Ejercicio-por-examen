using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.SceneManagement; // Para gestionar el cambio de escenas
using UnityEngine.UI; // Necesario para la clase Button

public class GameManager : MonoBehaviour
{
    // =====================================================================
    // ASIGNABLES DESDE EL INSPECTOR (solo elementos de la escena de juego)
    // =====================================================================
    public TMP_Text timerText; // Asigna aquí el TextMeshPro para mostrar el tiempo

    // =====================================================================
    // VARIABLES ESTÁTICAS (PERSISTEN ENTRE ESCENAS)
    // =====================================================================
    public static int currentLoggedInUserId = 0; // ID del usuario actualmente logueado
    public static string currentUserName = ""; // Nombre del usuario actualmente logueado
    public static int currentPlayedLevelId = 0; // ID del nivel que se está jugando (o 0 si no hay nivel específico)
    public static string currentPlayedLevelName = ""; // Nombre del nivel que se está jugando
    public static float lastScoreSubmitted = 0f; // Último puntaje de supervivencia registrado

    // =====================================================================
    // VARIABLES INTERNAS DEL SCRIPT
    // =====================================================================
    private float survivalTime = 0f; // Contador de tiempo de la partida actual
    private bool isGameActive = false; // Estado del juego (activo/inactivo)

    // URL base de tu servidor PHP. Debe coincidir con la de RegistroUsuario.cs
    private readonly string baseURL = "http://localhost/flappybird/";

    // =====================================================================
    // CLASES AUXILIARES PARA DESERIALIZAR RESPUESTAS PHP
    // =====================================================================
    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public string details;
    }

    // =====================================================================
    // MÉTODOS DE CICLO DE VIDA DE UNITY
    // =====================================================================
    void Start()
    {
        StartGame(); // Iniciar la partida al comienzo de la escena
    }

    void Update()
    {
        if (isGameActive)
        {
            survivalTime += Time.deltaTime; // Incrementar el tiempo cada frame
            if (timerText != null)
            {
                timerText.text = "Tiempo: " + survivalTime.ToString("F2") + "s"; // Mostrar el tiempo con 2 decimales
            }
        }
    }

    // =====================================================================
    // LÓGICA DEL JUEGO
    // =====================================================================
    public void StartGame()
    {
        survivalTime = 0f; // Reiniciar el contador de tiempo
        isGameActive = true; // El juego está activo
        Debug.Log("Juego iniciado en nivel: " + currentPlayedLevelName + " por usuario: " + currentUserName);
        // Aquí iría la lógica para reiniciar la posición del pájaro, tuberías, etc.
    }

    // Método que se llama cuando el jugador "muere" (desde el script del pájaro o ScoreTimer)
    public void PlayerDied()
    {
        if (!isGameActive) return; // Evitar múltiples llamadas si el juego ya terminó

        isGameActive = false; // Detener el juego
        Debug.Log("Jugador murió. Tiempo de supervivencia: " + survivalTime.ToString("F2") + "s");

        // Guardar el tiempo final en la variable estática para que la escena de Game Over lo pueda leer
        lastScoreSubmitted = survivalTime;

        // Enviar el puntaje a la base de datos (se inicia en segundo plano)
        // No esperamos aquí la respuesta del servidor para no causar demora.
        if (currentLoggedInUserId != 0 && currentPlayedLevelId != 0)
        {
            StartCoroutine(SubmitScoreCoroutine(currentLoggedInUserId, currentPlayedLevelId, survivalTime));
        }
        else
        {
            Debug.LogWarning("No se pudo enviar el puntaje: ID de usuario o nivel no válido. No se intentará guardar en DB.");
        }

        // ==================================================================================
        // ¡¡Cargar la escena de Game Over INMEDIATAMENTE!!
        // ==================================================================================
        SceneManager.LoadScene("GameOver"); // ¡IMPORTANTE! Reemplaza con el nombre exacto de tu escena de Game Over
    }

    // Corrutina para enviar el puntaje al servidor PHP
    private IEnumerator SubmitScoreCoroutine(int userId, int levelId, float timeSurvived)
    {
        WWWForm form = new WWWForm();
        form.AddField("usuario_id", userId);
        form.AddField("nivel_id", levelId);
        form.AddField("tiempo_sobrevivido", timeSurvived.ToString("F2")); // Formato a 2 decimales y convertir a string

        using (UnityWebRequest request = UnityWebRequest.Post(baseURL + "submit_score.php", form))
        {
            Debug.Log("Enviando puntaje a la base de datos en segundo plano...");
            // Inicia la solicitud y la corrutina principal continúa sin esperar
            request.SendWebRequest();

            // Esta corrutina simplemente terminará de ejecutarse después de un frame
            // mientras la petición web se procesa en segundo plano.
            yield return null; 
        }
    }

    // =====================================================================
    // MÉTODO ESTÁTICO PARA PASAR DATOS DESDE OTRAS ESCENAS (ej. Login/Registro)
    // =====================================================================
    public static void SetGameSessionData(int userId, string userName, int levelId, string levelName)
    {
        currentLoggedInUserId = userId;
        currentUserName = userName;
        currentPlayedLevelId = levelId;
        currentPlayedLevelName = levelName;
        Debug.Log("Datos de sesión del juego establecidos: User ID " + userId + ", Username " + userName + ", Level ID " + levelId + ", Level Name " + levelName);
    }
}
