using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Inicio : MonoBehaviour
{
    public void CargarNivel(int nivel)
    {
        SceneManager.LoadScene(nivel);
    }
}
