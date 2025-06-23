    using UnityEngine;
    using UnityEngine.Networking;
    using System.Collections;
    using System.Collections.Generic; // Necesario para List
    using TMPro; // Necesario para TextMeshPro InputField, Dropdown y Text
    using UnityEngine.SceneManagement; // Necesario para gestionar el cambio de escenas

    public class RegistroUsuario : MonoBehaviour
    {
        // =====================================================================================
        // VARIABLES PÚBLICAS ASIGNABLES DESDE EL INSPECTOR (SOLO PARA REGISTRO)
        // =====================================================================================

        public TMP_InputField inputNombreUsuario; // Asigna el InputField del nombre de usuario
        public TMP_InputField inputContrasena;    // Asigna el InputField de la contraseña
        public TMP_Dropdown dropdownAves;         // Asigna el Dropdown para seleccionar el ave
        public TMP_Text textoMensaje;             // Asigna el TextMeshPro para mostrar mensajes al usuario

        // =====================================================================================
        // VARIABLES INTERNAS DEL SCRIPT
        // =====================================================================================

        private readonly string baseURL = "http://localhost/flappybird/";

        // =====================================================================================
        // CLASES AUXILIARES PARA DESERIALIZAR LAS RESPUESTAS JSON DE PHP
        // =====================================================================================

        [System.Serializable]
        public class ResponseData
        {
            public string message;
            public string details;
            public int user_id;   // Asegúrate que tu PHP de registro devuelve el ID del usuario
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

        // =====================================================================================
        // MÉTODOS DE INICIALIZACIÓN DE UNITY
        // =====================================================================================

        void Start()
        {
            StartCoroutine(GetBirdsCoroutine());
        }

        // =====================================================================================
        // FUNCIONALIDAD PRINCIPAL: REGISTRO DE USUARIO
        // =====================================================================================

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

                        // ====================================================================
                        // ¡¡PASO CRUCIAL!! Pasar los datos al GameManager antes de cargar la escena de juego.
                        // Usamos el user_id devuelto por el PHP y un nivel por defecto para pruebas (ID 1).
                        // "Bosque Tranquilo" es el nombre del nivel por defecto (ID 1).
                        // ====================================================================
                        GameManager.SetGameSessionData(response.user_id, nombreUsuario, 1, "Bosque Tranquilo");

                        // Ahora, cargar la escena de juego.
                        // Asegúrate de que "FlappyEscena" sea el nombre EXACTO de tu escena de juego
                        // y que esté añadida en File > Build Settings.
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

        // =====================================================================================
        // OTRAS FUNCIONALIDADES: CARGAR AVES PARA EL DROPDOWN
        // =====================================================================================

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
    