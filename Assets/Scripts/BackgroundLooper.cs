using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public float speed = 2f;
    private float width;

    void Start()
    {
        // Obtiene el ancho real del sprite
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= -width)
        {
            // Lo mueve al final del otro fondo
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}
