using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Para usar TextMeshPro
using UnityEngine.UI;  // Para usar el componente Button

public class LevelSelector: MonoBehaviour
{
    public TMP_Text levelSelectionText;  // Título de la selección de niveles
    public Button nivel1Button;          // Botón para el primer nivel
    public Button nivel2Button;          // Botón para el segundo nivel
    public Button nivel3Button;          // Botón para el tercer nivel
    public Button nivel4Button;          // Botón para el cuarto nivel

    void Start()
    {
        // Asignar las funciones a los botones
        nivel1Button.onClick.AddListener(() => LoadLevel("Bosque Tranquilo"));
        nivel2Button.onClick.AddListener(() => LoadLevel("Cuevas Oscuras"));
        nivel3Button.onClick.AddListener(() => LoadLevel("Cielos Tormentosos"));
        nivel4Button.onClick.AddListener(() => LoadLevel("El Gran Cañón"));
    }

    void LoadLevel(string levelName)
    {
        // Cargar la escena del nivel seleccionado
        SceneManager.LoadScene(levelName);
    }
}
