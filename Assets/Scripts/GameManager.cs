using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;
    public static int currentLoggedInUserId = 0;
    public static string currentUserName = "";
    public static int currentPlayedLevelId = 0;
    public static string currentPlayedLevelName = "";
    public static float lastScoreSubmitted = 0f;
    public static string lastGameScene; // Variable para guardar la escena anterior
    private float survivalTime = 0f;
    private bool isGameActive = false;
    private readonly string baseURL = "http://localhost/flappybird/";

    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public string details;
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (isGameActive)
        {
            survivalTime += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "Tiempo: " + survivalTime.ToString("F2") + "s";
            }
        }
    }

    public void StartGame()
    {
        survivalTime = 0f;
        isGameActive = true;
    }

    public void PlayerDied()
    {
        if (!isGameActive) return;

        isGameActive = false;
        lastScoreSubmitted = survivalTime;

        // Guardar la escena actual antes de ir a GameOver
        lastGameScene = SceneManager.GetActiveScene().name;
        Debug.Log("Guardando escena: " + lastGameScene); // Para debug

        if (currentLoggedInUserId != 0 && currentPlayedLevelId != 0)
        {
            StartCoroutine(SubmitScoreCoroutine(currentLoggedInUserId, currentPlayedLevelId, survivalTime));
        }

        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator SubmitScoreCoroutine(int userId, int levelId, float timeSurvived)
    {
        WWWForm form = new WWWForm();
        form.AddField("usuario_id", userId);
        form.AddField("nivel_id", levelId);
        form.AddField("tiempo_sobrevivido", timeSurvived.ToString("F2"));

        using (UnityWebRequest request = UnityWebRequest.Post(baseURL + "submit_score.php", form))
        {
            request.SendWebRequest();
            yield return null;
        }
    }

    public static void SetGameSessionData(int userId, string userName, int levelId, string levelName)
    {
        currentLoggedInUserId = userId;
        currentUserName = userName;
        currentPlayedLevelId = levelId;
        currentPlayedLevelName = levelName;
    }
}