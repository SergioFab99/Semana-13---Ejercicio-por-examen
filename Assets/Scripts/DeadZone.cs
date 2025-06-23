using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreTimer gm = Object.FindFirstObjectByType<ScoreTimer>();
            if (gm != null)
            {
                gm.PlayerDied();
            }
        }
    }
}
