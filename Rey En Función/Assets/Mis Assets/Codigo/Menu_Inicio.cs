using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Inicio : MonoBehaviour
{
    public GameObject[] contenedores;
    int indiceActual;

    public void CargarNivel(int nivel)
    {
        SceneManager.LoadScene(nivel);
    }

    public void Cerrar()
    {
        Application.Quit();
    }

    public void Cambiar(int indice)
    {
        contenedores[indiceActual].SetActive(false);
        contenedores[indice].SetActive(true);
        indiceActual = indice;
    }
}
