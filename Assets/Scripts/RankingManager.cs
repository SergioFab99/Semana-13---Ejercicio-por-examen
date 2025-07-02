using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class RankingManager : MonoBehaviour
{
    public TMP_Text currentPlayerRankingText; // Para mostrar el puntaje del jugador actual
    public Transform rankingsContainer; // El contenedor donde se mostrarán los puntajes
    public TextMeshProUGUI rankingEntryText; // Usamos un TextMeshProUGUI para las entradas del ranking

    void Start()
    {
        GetRankingData(); // Obtener los datos del ranking
    }

    void GetRankingData()
    {
        StartCoroutine(FetchRankingData()); // Hacer la solicitud al servidor
    }

    private IEnumerator FetchRankingData()
    {
        string url = "http://localhost/flappybird/get_ranking.php"; // URL del PHP para obtener los puntajes

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                RankingResponse response = JsonUtility.FromJson<RankingResponse>(jsonResponse);

                if (response.message == "success")
                {
                    DisplayRanking(response.data);  // Mostrar el ranking
                    DisplayCurrentPlayerRank();     // Mostrar el puntaje del jugador actual
                }
                else
                {
                    Debug.LogError("Error al obtener los datos del ranking: " + response.details);
                }
            }
            else
            {
                Debug.LogError("Error en la solicitud HTTP: " + request.error);
            }
        }
    }

    void DisplayRanking(RankingEntry[] rankingData)
    {
        foreach (RankingEntry entry in rankingData)
        {
            // Crear una nueva entrada para cada jugador
            TMP_Text newEntry = Instantiate(rankingEntryText, rankingsContainer);
            newEntry.text = $"{entry.username}: {entry.time} s";  // Asignar el nombre y tiempo al TextMeshPro
        }
    }

    void DisplayCurrentPlayerRank()
    {
        // Mostrar el puntaje del jugador actual al principio
        currentPlayerRankingText.text = $"{GameManager.currentUserName} - {GameManager.lastScoreSubmitted.ToString("F2")} s";
    }

    [System.Serializable]
    public class RankingResponse
    {
        public string message;
        public string details;
        public RankingEntry[] data;
    }

    [System.Serializable]
    public class RankingEntry
    {
        public string username;
        public float time;
    }
}
