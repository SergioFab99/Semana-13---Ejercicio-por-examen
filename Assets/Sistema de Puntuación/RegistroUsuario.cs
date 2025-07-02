using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class RegistroUsuario : MonoBehaviour
{
    public TMP_InputField inputNombreUsuario;
    public TMP_InputField inputContrasena;
    public TMP_Dropdown dropdownAves;
    public TMP_Text textoMensaje;

    private readonly string baseURL = "http://localhost/flappybird/";

    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public string details;
        public int user_id;
    }

    [System.Serializable]
    public class BirdsResponse
    {
        public string message;
        public string details;
        public BirdData[] data;
    }

    [System.Serializable]
    public class BirdData
    {
        public int id;
        public string nombre;
    }

    void Start()
    {
        StartCoroutine(GetBirdsCoroutine());
    }

    public void Registrar()
    {
        string nombre = inputNombreUsuario.text.Trim();
        string contrasena = inputContrasena.text.Trim();
        int aveId = dropdownAves.value + 1;

        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contrasena))
        {
            textoMensaje.text = "Completa todos los campos.";
            return;
        }

        StartCoroutine(RegisterUserCoroutine(nombre, contrasena, aveId));
    }

    private IEnumerator RegisterUserCoroutine(string nombreUsuario, string contrasena, int aveId)
    {
        WWWForm form = new WWWForm();
        form.AddField("nombre_usuario", nombreUsuario);
        form.AddField("contrasena", contrasena);
        form.AddField("ave_id", aveId);

        using (UnityWebRequest request = UnityWebRequest.Post(baseURL + "registro_usuario.php", form))
        {
            textoMensaje.text = "Registrando...";
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Respuesta del servidor (Registro): " + responseText);
                ResponseData response = JsonUtility.FromJson<ResponseData>(responseText);

                if (response.message == "success")
                {
                    textoMensaje.text = "¡Usuario registrado exitosamente!";
                    inputNombreUsuario.text = "";
                    inputContrasena.text = "";

                    GameManager.SetGameSessionData(response.user_id, nombreUsuario, 1, "Bosque Tranquilo");
                    SceneManager.LoadScene("FlappyEscena");
                }
                else
                {
                    textoMensaje.text = "Error de registro: " + response.details;
                }
            }
            else
            {
                Debug.LogError("Error en la solicitud web (Registro): " + request.error);
                textoMensaje.text = "Error de conexión o problema en el servidor: " + request.error;
            }
        }
    }

    private IEnumerator GetBirdsCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseURL + "get_birds.php"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Respuesta de Aves: " + responseText);
                BirdsResponse birdsResponse = JsonUtility.FromJson<BirdsResponse>(responseText);

                if (birdsResponse.message == "success" && birdsResponse.data != null)
                {
                    dropdownAves.ClearOptions();
                    List<string> birdNames = new List<string>();
                    foreach (BirdData bird in birdsResponse.data)
                    {
                        birdNames.Add(bird.nombre);
                    }
                    dropdownAves.AddOptions(birdNames);
                    textoMensaje.text = "Aves cargadas.";
                }
                else
                {
                    textoMensaje.text = "Error al cargar aves: " + birdsResponse.details;
                }
            }
            else
            {
                Debug.LogError("Error al obtener aves: " + request.error);
                textoMensaje.text = "Error de conexión al cargar aves: " + request.error;
            }
        }
    }
}