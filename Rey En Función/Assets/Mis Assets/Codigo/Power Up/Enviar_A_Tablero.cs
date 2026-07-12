using UnityEngine;

public class Enviar_A_Tablero : MonoBehaviour
{
    public int efecto;
    public Color32[] color;
    MeshRenderer rend;

    public delegate void EnviarValor(int valor);
    public static event EnviarValor enviar;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();

        rend.material.color = color[efecto];
    }

    private void OnTriggerEnter(Collider other)
    {
        enviar.Invoke(efecto);
        gameObject.SetActive(false);
    }
}
