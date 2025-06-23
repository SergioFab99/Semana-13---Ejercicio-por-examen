using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 2f;
    public float verticalRange = 4f; // Ajusta esto según el tamaño del fondo
    public float obstacleLifetime = 3f; // Tiempo antes de destruir el objeto

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSingleObstacle();
            timer = 0f;
        }
    }

    void SpawnSingleObstacle()
    {
        float camSize = Camera.main.orthographicSize;
        float maxY = Mathf.Min(camSize, verticalRange);
        float minY = -maxY;

        float yOffset = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, yOffset, 0f);

        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        Destroy(newObstacle, obstacleLifetime); // 🔥 Destruir en 3 segundos
    }
}
