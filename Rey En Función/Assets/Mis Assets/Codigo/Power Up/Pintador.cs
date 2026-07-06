using UnityEngine;

public class Pintador : MonoBehaviour
{
    public Color32[] colores;
    public float intervaloDeCambio;
    Renderer rend;
    int objetivo, actual;
    Color32 colorFinal;

    void Start()
    {
        rend = GetComponent<Renderer>();

        actual = Random.Range(0, 2) * 2;

        objetivo = 1;

        rend.material.color = colores[actual];
        colorFinal = colores[actual];

        Invoke("CambiarObjetivo", Random.Range(intervaloDeCambio * .75f, intervaloDeCambio * 1.25f));
    }

    void Update()
    {
        colorFinal = Color32.Lerp(colores[objetivo], colorFinal, Time.deltaTime);
        rend.material.color = colorFinal;
    }

    void CambiarObjetivo()
    {
        if (objetivo == 0 || objetivo == 2)
        {
            actual = objetivo;
            objetivo = 1;
        }
        else if (objetivo == 1 && actual == 0)
        {
            actual = objetivo;
            objetivo = 2;
        }
        else
        {
            actual = objetivo;
            objetivo = 0;
        }

        Invoke("CambiarObjetivo", Random.Range(intervaloDeCambio * .75f, intervaloDeCambio * 1.25f));
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
